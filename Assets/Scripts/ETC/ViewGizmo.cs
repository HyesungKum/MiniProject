using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewGizmo : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, this.transform.lossyScale);
    }
}
