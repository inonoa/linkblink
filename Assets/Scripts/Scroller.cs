using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Scroller : MonoBehaviour
{
    [SerializeField] float upBound = 0;
    [SerializeField] float downBound = -1000;
    [SerializeField] float force = 10;

    [SerializeField] [Range(0, 1)] float smoothness = 0.9f;

    [SerializeField] [ReadOnly] float wheelRot = 0;

    void Start()
    {
        
    }


    void Update()
    {
        wheelRot = Mathf.Lerp(wheelRot, Input.GetAxisRaw("Mouse ScrollWheel"), 1 - smoothness);
        Vector3 pos = transform.localPosition;
        pos.y = Mathf.Clamp(pos.y - wheelRot * force, downBound, upBound);
        transform.localPosition = pos;
    }
}
