using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;

public class InfoCanvas : MonoBehaviour {

    [SerializeField]
    private Text title;
    [SerializeField]
    private Text infoText;
    [SerializeField]
    private VRInteractiveItem interactiveItem;
    [SerializeField]
    private Menu menu;

    public void ShowProjectDetails(Project project)
    {
        title.text = project.Name;
        infoText.text = project.Description+ "\n\nTechnologies: "+string.Join(", ", project.Technologies.ToArray());
    }

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
        menu.ShowProjectTiles();
    }
}
