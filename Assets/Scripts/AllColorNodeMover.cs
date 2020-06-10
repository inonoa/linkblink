using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllColorNodeMover : NodeMover
{
    void Awake(){
        _Colors = Enum.GetValues(typeof(NodeColor)) as NodeColor[];
    }

    public void SelectColor(NodeColor color){
        //
    }

    public void UnSelectColor(){
        //
    }
}
