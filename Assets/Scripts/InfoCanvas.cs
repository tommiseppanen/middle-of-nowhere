using UnityEngine;
using UnityEngine.UI;

public class InfoCanvas : MonoBehaviour
{
    [SerializeField]
    private Text _title;
    [SerializeField]
    private Text _infoText;

    public void ShowProjectDetails(Project project)
    {
        _title.text = project.Name;
        _infoText.text = project.Description+ "\n\nTechnologies: "+string.Join(", ", project.Technologies.ToArray());
    }
}
