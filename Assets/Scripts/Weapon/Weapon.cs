using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Weapon : MonoBehaviour
{
    protected Collider weaponColl = null;

    protected void Awake()
    {
        weaponColl = this.GetComponent<Collider>();
        weaponColl.isTrigger = true;
    }

    protected void Update()
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
