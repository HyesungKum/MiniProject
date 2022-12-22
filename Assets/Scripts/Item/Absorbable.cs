using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Absorbable : MonoBehaviour
{
    [field: SerializeField] float AbsorbSpeed { get; set; }


    protected void OnTriggerStay (Collider other)
    {
        if (other.name == nameof(AbsorbRange))
        {
            this.transform.LookAt(other.transform);
            this.transform.position += transform.forward*Time.deltaTime*AbsorbSpeed;
        }
    }

    protected abstract void OnTriggerEnter(Collider other);
}
