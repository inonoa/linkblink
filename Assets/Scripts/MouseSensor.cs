using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MouseSensor : TouchableByMouse
{
    public bool IsTouched{ get; private set; } = false;
    public bool MouseIn{ get; private set; } = false;
    public bool MouseOut{ get; private set; } = false;
    public override bool ShutOutRay => false;

    public event EventHandler<MouseEventArgs> OnMouseOn;
    public event EventHandler<MouseEventArgs> OnMouseOut;

    void Update()
    {
        Vector3 mousePos;
        bool isTouchedNow = ClickRayCaster.Instance.MouseOn(this, out mousePos);
        if(! IsTouched && isTouchedNow){
            OnMouseOn?.Invoke(this, new MouseEventArgs(true, mousePos));
            MouseIn = true;
        }else{
            MouseIn = false;
        }
        if(IsTouched && ! isTouchedNow){
            OnMouseOut?.Invoke(this, new MouseEventArgs(false, new Vector3()));
            MouseOut = false;
        }else{
            MouseOut = false;
        }
        IsTouched = isTouchedNow;
    }
}

public class MouseEventArgs : EventArgs{
    
    public bool MouseOn{ get; private set; }
    public Vector3 Point{ get; private set; }
    public MouseEventArgs(bool mouseOn, Vector3 point){
        this.Point = point;
        this.MouseOn = mouseOn;
    }
}
