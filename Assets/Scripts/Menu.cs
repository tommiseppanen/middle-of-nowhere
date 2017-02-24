using Newtonsoft.Json;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {
    [SerializeField]
    private string Url;
    [SerializeField]
    private Transform TilePrefab;
    [SerializeField]
    private Transform TileParent;
    [SerializeField]
    private InfoCanvas InfoCanvas;
    [SerializeField]
    private TextMesh currentPage;

    private const int TilesPerPage = 6;
    private const float TileGap = 1.2f;
    private const float HeightOffset = 1.0f;
    private const float PageWidth = 3.6f;
    private static readonly Quaternion TileRotation = Quaternion.Euler(90, 180, 0);

    private int page;
    private int maxLoadedPage;
    private List<Transform> tiles;
    private float prevAxisValue;

    IEnumerator Start () {
        tiles = new List<Transform>();
        var dataRequest = new WWW(Url);
        yield return dataRequest;
        var projects = JsonConvert.DeserializeObject<List<Project>>(dataRequest.text)
            .OrderByDescending(p => p.Published).ToList();
        createTiles(projects);
        UpdatePageNumber();
    }

    private void createTiles(List<Project> projects)
    {
        for (int i = 0; i < projects.Count; i++)
        {
            var tileObject = Instantiate(TilePrefab, 
                new Vector3((i / 2) * TileGap - TileGap, ((i + 1) % 2) * TileGap + HeightOffset, 0), 
                TileRotation, TileParent);
            tiles.Add(tileObject);
            InitializeTile(tileObject, projects[i], i < 6);
        }
    }

    private void InitializeTile(Transform tileObject, Project project, bool setActive)
    {
        var tile = tileObject.GetComponent<Tile>();
        tile.gameObject.SetActive(setActive);
        tile.ProjectData = project;
        tile.Menu = this;
        StartCoroutine(tile.LoadImage());
    }

    private void UpdatePageNumber()
    {
        currentPage.text = (page + 1) + "/" + PageCount();
    }

    private int PageCount()
    {
        return Mathf.CeilToInt((float)tiles.Count / TilesPerPage);
    }
	
	void Update ()
    {
        var axisValue = Input.GetAxis("Horizontal");
        if (axisValue != prevAxisValue)
        {
            if (axisValue > 0.5f)
                ChangePage(1);
            else if (axisValue < -0.5f)
                ChangePage(-1);
            prevAxisValue = axisValue;
        }
    }

    public void ChangePage(int delta)
    {
        if (page + delta < 0 || page + delta >= PageCount())
            return;
        ToggleVisibility(page, false);
        page += delta;
        TileParent.transform.position = new Vector3(-PageWidth * page, 0, 0);
        ToggleVisibility(page, true);
        UpdatePageNumber();
    }

    private void ToggleVisibility(int page, bool visible)
    {
        for (int i = 0; i < TilesPerPage; i++)
        {
            var tileIndex = page * TilesPerPage + i;
            if (tileIndex < tiles.Count)
            {
                tiles[tileIndex].gameObject.SetActive(visible);
                tiles[tileIndex].gameObject.layer = LayerMask.NameToLayer("Default");
            }
            else
                break;
        }
    }

    public void ShowProjectDetails(Project project)
    {
        InfoCanvas.ShowProjectDetails(project);
        InfoCanvas.gameObject.SetActive(true);
        TileParent.gameObject.SetActive(false);
    }

    public void ShowProjectTiles()
    {
        InfoCanvas.gameObject.SetActive(false);
        TileParent.gameObject.SetActive(true);
    }
}
