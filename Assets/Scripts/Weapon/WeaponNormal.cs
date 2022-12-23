using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class WeaponNormal : Weapon
{
    [SerializeField] float damage;
    [Tooltip("second")]
    [SerializeField] float Speed;
    [SerializeField] float duration;

    private float Timer = 0f;

    public override void MoveProd()
    {
        Timer += Time.deltaTime;

        if (Timer >= duration) Destroy(this.gameObject);

        this.transform.position += -this.transform.right * Time.deltaTime * Speed;
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.name.Split("Monster").Length == 2)
        {
            other.SendMessage("Damage", damage, SendMessageOptions.DontRequireReceiver);
            Destroy(this.gameObject);
        }
    }
}
