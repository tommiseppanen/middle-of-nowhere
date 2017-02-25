using UnityEngine;
using VRStandardAssets.Utils;

public class InteractiveItemBase : VRInteractiveItem
{
    private void OnEnable()
    {
        OnClick += HandleClick;
        OnOver += HandleOver;
        OnOut += HandleOut;
    }

    private void OnDisable()
    {
        OnClick -= HandleClick;
        OnOver -= HandleOver;
        OnOut -= HandleOut;
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

