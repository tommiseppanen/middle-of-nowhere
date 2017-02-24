using UnityEngine;
using VRStandardAssets.Utils;

public class InteractiveItemBase : MonoBehaviour
{
    [SerializeField]
    private VRInteractiveItem interactiveItem;

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

    private void HandleOver()
    {
        gameObject.layer = LayerMask.NameToLayer("Outline");
    }

    private void HandleOut()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    protected virtual void HandleClick()
    {
    }
}

