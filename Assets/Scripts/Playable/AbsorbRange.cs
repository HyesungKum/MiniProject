using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class AbsorbRange : MonoBehaviour
{
    public SphereCollider Collider = null;

    private void Awake()
    {
        Collider = this.GetComponent<SphereCollider>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.5f, 0f, 0f, 0.7f);
        Gizmos.DrawSphere(this.transform.position, Collider.radius);
    }
}
