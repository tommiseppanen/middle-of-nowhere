using System.Collections;
using UnityEngine;
using VRStandardAssets.Utils;

public class Tile : MonoBehaviour {

    [SerializeField]
    private VRInteractiveItem interactiveItem;

    public Menu Menu { get; set; }
    public Project ProjectData { get; set; }

    private void OnEnable()
    {
        interactiveItem.OnClick += HandleClick;
        interactiveItem.OnOver += HandleOver;
        interactiveItem.OnOut += HandleOut;
    }


    private void OnDisable()
    {
        interactiveItem.OnClick -= HandleClick;
        interactiveItem.OnOver -= HandleOver;
        interactiveItem.OnOut -= HandleOut;
    }

    public IEnumerator LoadImage()
    {
        var renderer = GetComponent<Renderer>();
        WWW imageRequest = new WWW(ProjectData.ImageUrl);
        yield return imageRequest;
        SetMaterialParameters(renderer.material, imageRequest.texture);
    }

    private void SetMaterialParameters(Material target, Texture2D texture)
    {
        target.mainTexture = texture;
        var height = texture.height;
        var width = texture.width;
        if (height > width)
        {
            var yScale = (float)width / height;
            target.mainTextureScale = new Vector2(1.0f, yScale);
            target.mainTextureOffset = new Vector2(0.0f, 1.0f - yScale);
        }
        else
            target.mainTextureScale = new Vector2((float)height / width, 1.0f);
    }

    void HandleOver()
    {
        gameObject.layer = LayerMask.NameToLayer("Outline");
    }

    void HandleOut()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    void HandleClick()
    {
        Menu.ShowProjectDetails(ProjectData);
        gameObject.layer = LayerMask.NameToLayer("Default");
    }
}
