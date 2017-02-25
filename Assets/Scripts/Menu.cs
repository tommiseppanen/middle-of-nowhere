using Newtonsoft.Json;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {
    [SerializeField]
    private string _url;
    [SerializeField]
    private Transform _tilePrefab;
    [SerializeField]
    private Transform _tileParent;
    [SerializeField]
    private InfoCanvas _infoCanvas;
    [SerializeField]
    private TextMesh _currentPage;

    private const int TilesPerPage = 6;
    private const float TileGap = 1.2f;
    private const float HeightOffset = 1.0f;
    private const float PageWidth = 3.6f;
    private static readonly Quaternion TileRotation = Quaternion.Euler(90, 180, 0);

    private int _page;
    private List<Transform> _tiles;
    private bool _pageChangeTriggered;

    private IEnumerator Start () {
        _tiles = new List<Transform>();
        var dataRequest = new WWW(_url);
        yield return dataRequest;
        var projects = JsonConvert.DeserializeObject<List<Project>>(dataRequest.text)
            .OrderByDescending(p => p.Published).ToList();
        CreateTiles(projects);
        UpdatePageNumber();
    }

    private void CreateTiles(IList<Project> projects)
    {
        for (var i = 0; i < projects.Count; i++)
        {
            var tileObject = Instantiate(_tilePrefab, 
                new Vector3((i / 2) * TileGap - TileGap, ((i + 1) % 2) * TileGap + HeightOffset, 0), 
                TileRotation, _tileParent);
            _tiles.Add(tileObject);
            InitializeTile(tileObject, projects[i], i < 6);
        }
    }

    private void InitializeTile(Component tileObject, Project project, bool setActive)
    {
        var tile = tileObject.GetComponent<Tile>();
        tile.gameObject.SetActive(setActive);
        tile.ProjectData = project;
        tile.Menu = this;
        StartCoroutine(tile.LoadImage());
    }

    private void UpdatePageNumber()
    {
        _currentPage.text = (_page + 1) + "/" + PageCount();
    }

    private int PageCount()
    {
        return Mathf.CeilToInt((float)_tiles.Count / TilesPerPage);
    }

    private void Update ()
    {
        var axisValue = Input.GetAxis("Horizontal");
        if (!_pageChangeTriggered)
        {
            if (axisValue > 0.5f)
            {
                ChangePage(1);
                _pageChangeTriggered = true;
            }
            else if (axisValue < -0.5f)
            {
                ChangePage(-1);
                _pageChangeTriggered = true;
            }
        }
        else
        {
            if(axisValue < 0.5f && axisValue > -0.5f)
                _pageChangeTriggered = false;
        }
    }

    public void ChangePage(int delta)
    {
        if (_page + delta < 0 || _page + delta >= PageCount())
            return;
        ToggleVisibility(_page, false);
        _page += delta;
        _tileParent.transform.position = new Vector3(-PageWidth * _page, 0, 0);
        ToggleVisibility(_page, true);
        UpdatePageNumber();
    }

    private void ToggleVisibility(int page, bool visible)
    {
        for (var i = 0; i < TilesPerPage; i++)
        {
            var tileIndex = page * TilesPerPage + i;
            if (tileIndex < _tiles.Count)
            {
                _tiles[tileIndex].gameObject.SetActive(visible);
                _tiles[tileIndex].gameObject.layer = LayerMask.NameToLayer("Default");
            }
            else
                break;
        }
    }

    public void ShowProjectDetails(Project project)
    {
        _infoCanvas.ShowProjectDetails(project);
        _infoCanvas.gameObject.SetActive(true);
        _tileParent.gameObject.SetActive(false);
    }

    public void ShowProjectTiles()
    {
        _infoCanvas.gameObject.SetActive(false);
        _tileParent.gameObject.SetActive(true);
    }
}
