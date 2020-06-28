using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "AllColorElement", menuName = "ScriptableObjects/AllColorElement", order = 7)]
public class AllColorElement : NewElement
{
    protected override bool ExistIn(StageData data){
        return data.Rows.Any(row => row.Nodes.Any(node => node == NodeType.AllColor));
    }

    public AllColorElement(){
        NewElement._Elements.Add(this);
    }
}
