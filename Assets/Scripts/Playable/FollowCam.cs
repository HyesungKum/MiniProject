using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [SerializeField] GameObject target = null;
    private Transform targetTr = null;

    private void Awake()
    {
        if (target == null)
        {
            target = FindObjectOfType<Playable>().gameObject;
        }
        else
        {
            targetTr = target.transform;
        }
    }

    private void Update()
    {
        Vector3 newPos = new Vector3(targetTr.position.x, this.transform.position.y, targetTr.position.z);
        this.transform.position = newPos;
    }
}
