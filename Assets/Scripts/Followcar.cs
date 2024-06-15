using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Followcar : MonoBehaviour
{
    public Transform car; // Reference to the car's transform
    public Vector3 positionOffset; // Offset from the car's position
    public Vector3 rotationOffset; // Offset from the car's rotation
    void Start()
    {
        // Remove all colliders
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            Destroy(col);
        }

        // Remove all rigidbodies
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rigidbodies)
        {
            Destroy(rb);
        }
    }
    void LateUpdate()
    {
        if (car != null)
        {
            // Update position
            transform.position = car.position + car.TransformVector(positionOffset);
            // Update rotation
            transform.rotation = car.rotation * Quaternion.Euler(rotationOffset);
        }
    }
}
