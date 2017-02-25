using UnityEngine;

public class BackToMenuButton : InteractiveItemBase
{
    [SerializeField]
    private Menu _menu;

    protected override void HandleClick()
    {
        _menu.ShowProjectTiles();
    }
}
