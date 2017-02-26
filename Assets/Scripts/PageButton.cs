using UnityEngine;

namespace MiddleOfNowhere.Scripts
{
    public class PageButton : InteractiveItemBase
    {
        [SerializeField]
        private Menu _menu;
        [SerializeField]
        private int _pageDelta;

        protected override void HandleClick()
        {
            _menu.ChangePage(_pageDelta);
        }
    }
}

