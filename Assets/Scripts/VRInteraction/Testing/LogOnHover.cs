using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LogOnHover : MonoBehaviour
{
    private XRBaseInteractable interactable;

    private void Awake()
    {
        interactable = GetComponent<XRBaseInteractable>();

        if (interactable == null)
        {
            Debug.LogError("No XRBaseInteractable component found on " + gameObject.name);
            return;
        }

        interactable.hoverEntered.AddListener(OnHover);
    }

    private void OnDestroy()
    {
        if (interactable != null)
        {
            interactable.hoverEntered.RemoveListener(OnHover);
        }
    }

    private void OnHover(HoverEnterEventArgs args)
    {
        Debug.Log("Object hovered: " + gameObject.name);
    }
}
