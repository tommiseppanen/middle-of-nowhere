using UnityEngine;
using UnityEngine.UI;

public class InfoCanvas : InteractiveItemBase
{
    [SerializeField]
    private Text _title;
    [SerializeField]
    private Text _infoText;
    [SerializeField]
    private Menu _menu;

    public void ShowProjectDetails(Project project)
    {
        _title.text = project.Name;
        _infoText.text = project.Description+ "\n\nTechnologies: "+string.Join(", ", project.Technologies.ToArray());
    }

    protected override void HandleClick()
    {
        _menu.ShowProjectTiles();
    }
}
