using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarCheckpointSystem : MonoBehaviour
{
    private GameManager gameManager;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    void OnTriggerEnter(Collider other)
    {
        gameManager.PlayerCarEntersTrigger(other);
    }
}
