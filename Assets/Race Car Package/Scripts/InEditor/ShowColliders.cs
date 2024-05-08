using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ShowCollider : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, transform.lossyScale);
    }
}