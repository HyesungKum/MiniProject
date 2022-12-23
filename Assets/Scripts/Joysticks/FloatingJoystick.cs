using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using KHS_Axis;

namespace KHS_Axis
{
    static public class Axis
    {
        static public float GetHorizontal;
        static public float GetVertical;
    }
}

public class FloatingJoystick : Joystick
{
    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
    }

    private void Update()
    {
        KHS_Axis.Axis.GetHorizontal = Horizontal;
        KHS_Axis.Axis.GetVertical = Vertical;
    }
}