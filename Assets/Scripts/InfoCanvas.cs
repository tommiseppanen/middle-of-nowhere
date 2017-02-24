using UnityEngine;
using UnityEngine.UI;

public class InfoCanvas : InteractiveItemBase
{
    [SerializeField]
    private Text title;
    [SerializeField]
    private Text infoText;
    [SerializeField]
    private Menu menu;

    public void ShowProjectDetails(Project project)
    {
        title.text = project.Name;
        infoText.text = project.Description+ "\n\nTechnologies: "+string.Join(", ", project.Technologies.ToArray());
    }

    protected override void HandleClick()
    {
        menu.ShowProjectTiles();
    }
}
