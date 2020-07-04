using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "BombElement", menuName = "ScriptableObjects/BombElement", order = 5)]
public class BombElement : NewElement
{

    protected override bool ExistIn(StageData data){
        return data.Rows.Any(row => row.Nodes.Contains(NodeType.BombCyan));
    }
}
