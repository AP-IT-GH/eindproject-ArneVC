using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CarInteraction : XRBaseInteractable
{
    protected override void OnEnable()
    {
        base.OnEnable();
        hoverEntered.AddListener(OnHover);
        selectEntered.AddListener(OnGrab);
    }

    protected override void OnDisable()
    {
        hoverEntered.RemoveListener(OnHover);
        selectEntered.RemoveListener(OnGrab);
        base.OnDisable();
    }

    private void OnHover(HoverEnterEventArgs args)
    {
        Debug.Log("Hovered over: " + gameObject.name);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        Debug.Log("Selected: " + gameObject.name);
    }
}
