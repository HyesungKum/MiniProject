using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpItem : Absorbable
{
    [SerializeField] int exp = 10;
    [SerializeField] float rotSpeed = 4f;

    private void Update()
    {
        this.transform.Rotate(0f, Time.deltaTime*rotSpeed, 0f);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.name == nameof(Playable))
        {
            other.SendMessage("GetExp", exp, SendMessageOptions.DontRequireReceiver);
            ObjectPool.Inst.DestroyObject(this.gameObject);
        }
    }
}
