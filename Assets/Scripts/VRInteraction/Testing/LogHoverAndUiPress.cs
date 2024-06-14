using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LogHoverAndUiPress : XRBaseInteractable
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
        Debug.Log("Object hovered: " + gameObject.name);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        Debug.Log("Grabbed object: " + gameObject.name);
    }
}
