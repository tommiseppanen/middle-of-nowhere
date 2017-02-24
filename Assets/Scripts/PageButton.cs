using UnityEngine;

public class PageButton : InteractiveItemBase
{
    [SerializeField]
    private Menu menu;
    [SerializeField]
    private int pageDelta;

    protected override void HandleClick()
    {
        menu.ChangePage(pageDelta);
    }
}
