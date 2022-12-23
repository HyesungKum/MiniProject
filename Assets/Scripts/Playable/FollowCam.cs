using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [SerializeField] GameObject target = null;
    [SerializeField] float followSpeed = 20f;
    private Transform targetTr = null;

    private void Awake()
    {
        if (target == null)
        {
            target = GameObject.Find(nameof(Playable));
        }
        else
        {
            targetTr = target.transform;
        }
    }

    private void Update()
    {
        Vector3 newPos = new Vector3(targetTr.position.x, this.transform.position.y, targetTr.position.z);
        this.transform.position = Vector3.Lerp(this.transform.position, newPos, Time.deltaTime * followSpeed);
    }
}
