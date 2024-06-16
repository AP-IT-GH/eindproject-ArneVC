using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Followcar : MonoBehaviour
{
    public Transform car; // Reference to the car's transform
    public Vector3 positionOffset = new Vector3(0f, 0f, 0f); // Offset from the car's position
    public Vector3 rotationOffset = new Vector3(0f, 0f, 0f); // Offset from the car's rotation
    void Start()
    {
        //make player child of car
        transform.SetParent(car.transform);


        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
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
    void Update()
    {
        if (car != null)
        {
            // Update position
            transform.position = car.position + positionOffset;
            // Update rotation
            transform.rotation = car.rotation * Quaternion.Euler(rotationOffset);
        }
    }
}
