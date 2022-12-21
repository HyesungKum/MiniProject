#define PC
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playable : MonoBehaviour
{
    [SerializeField] private float PlayerSpeed;

    private void Awake()
    {
        PlayerSpeed = 10f;
    }

    void Update()
    {
        InputControll();
    }

    public void InputControll()
    {
        Vector3 moveDir = Vector3.zero;

        #region PC Controll
        #if PC
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                moveDir = new Vector3((hit.point.x - this.transform.position.x),0f, (hit.point.z - this.transform.position.z)).normalized;
            }
        }
        #endif
        #endregion
        this.transform.Translate(PlayerSpeed * Time.deltaTime * moveDir);

        #region Mobile Controll
#if Moblie
        if (Input.GetMouseButton(0))
        {
        }
#endif
        #endregion
    }
}
