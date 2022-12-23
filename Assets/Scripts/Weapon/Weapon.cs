using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Weapon : MonoBehaviour
{
    private Collider weaponColl = null;

    private void Awake()
    {
        weaponColl = this.GetComponent<Collider>();
        weaponColl.isTrigger = true;
    }

    public virtual void Update()
    {
        MoveProd();
    }

    /// <summary>
    /// need function about weapon interating with monster
    /// </summary>
    /// <param name="other">target will be monster</param>
    public abstract void OnTriggerEnter(Collider other);

    /// <summary>
    /// weapon objet and another movable function
    /// </summary>
    public abstract void MoveProd();
}
