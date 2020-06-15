using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Scroller : MonoBehaviour
{
    Canvas canvas;
    [SerializeField] float upBound = 0;
    [SerializeField] float downBound = -1000;
    [SerializeField] float force = 10;

    [SerializeField] [Range(0, 1)] float smoothness = 0.9f;
    [SerializeField] [ReadOnly] float wheelRot = 0;


    Vector2? lastMousePos;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
    }


    void Update()
    {

        Vector2 mousePos = Input.mousePosition;
        
        if(Input.GetMouseButton(0)){

            if(lastMousePos != null){
                Vector3 pos_ = transform.localPosition;
                pos_.y = Mathf.Clamp(pos_.y + mousePos.y - lastMousePos.Value.y, downBound, upBound);
                transform.localPosition = pos_;
            }
            lastMousePos = mousePos;
        }else{
            lastMousePos = null;
        }


#if UNITY_ANDROID
        
#else
        // wheelRot = Mathf.Lerp(wheelRot, Input.GetAxisRaw("Mouse ScrollWheel"), 1 - smoothness);
        // Vector3 pos = transform.localPosition;
        // pos.y = Mathf.Clamp(pos.y - wheelRot * force, downBound, upBound);
        // transform.localPosition = pos;
#endif
    }
}
