using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CircleButton : Button, ICanvasRaycastFilter
{
    [SerializeField] float radius = 50;

    public bool IsRaycastLocationValid(Vector2 sp, Camera camera){
        Vector2 screenPoint = camera.WorldToScreenPoint(transform.position);
        return Vector2.Distance(sp, screenPoint) < radius;
    }
}
