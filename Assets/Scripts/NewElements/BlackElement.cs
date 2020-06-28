using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "BlackElement", menuName = "ScriptableObjects/BlackElement", order = 5)]
public class BlackElement : NewElement
{
    protected override bool ExistIn(StageData data){
        return data.Rows.Any(row => row.Nodes.Any(node => node == NodeType.Black));
    }
}
