using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "RainbowElement", menuName = "ScriptableObjects/RainbowElement", order = 5)]
public class RainbowElement : NewElement
{
    protected override bool ExistIn(StageData data){
        return data.Rows.Any(row => row.Nodes.Any(node => node.HasTwoColors()));
    }

}
