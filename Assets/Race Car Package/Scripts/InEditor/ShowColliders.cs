using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ShowCollider : MonoBehaviour
{
    public bool isInvisibleBarrier = true;
    void OnDrawGizmos()
    {
        if(isInvisibleBarrier)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }
        Gizmos.matrix = this.transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}