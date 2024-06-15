using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CarInteraction : XRBaseInteractable
{
    private GameManager gameManager;
    protected override void OnEnable()
    {
        base.OnEnable();
        hoverEntered.AddListener(OnHover);
        hoverExited.AddListener(OnHoverExited);
        selectEntered.AddListener(OnGrab);

        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found");
        }
    }

    protected override void OnDisable()
    {
        hoverEntered.RemoveListener(OnHover);
        selectEntered.RemoveListener(OnGrab);
        base.OnDisable();
    }

    private void OnHover(HoverEnterEventArgs args)
    {
        if(gameManager != null)
        {
            gameManager.CarHover(gameObject);
        }
    }
    private void OnHoverExited(HoverExitEventArgs args)
    {
        if (gameManager != null)
        {
            gameManager.CarHoverExit(gameObject);
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (gameManager != null)
        {
            gameManager.CarSelect(gameObject);
        }
    }
}
