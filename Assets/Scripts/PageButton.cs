using UnityEngine;
using VRStandardAssets.Utils;

public class PageButton : MonoBehaviour
{
    [SerializeField]
    private VRInteractiveItem interactiveItem;
    [SerializeField]
    private Menu menu;
    [SerializeField]
    private int pageDelta;

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
        menu.ChangePage(pageDelta);
    }
}
