using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpItem : Absorbable
{
    [SerializeField] int exp = 10;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.name == nameof(Playable))
        {
            other.SendMessage("GetExp", exp, SendMessageOptions.DontRequireReceiver);
            Destroy(this.gameObject);
        }
    }
}
