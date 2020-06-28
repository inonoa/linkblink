using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "DiferrentNumbersElement", menuName = "ScriptableObjects/DifferentNumbersElement", order = 5)]
public class DifferentNumbersElement : NewElement
{

    protected override bool ExistIn(StageData data){
        
        //厳密にはこれではだめだがまあ……
        var nums = new Dictionary<NodeType, int>();
        var nodes = data.ToNodesList();
        foreach(var nts in nodes){
            foreach(NodeType nt in nts){
                if(nt == NodeType.None) continue;
                nums[nt] = nums.ContainsKey(nt) ? nums[nt] + 1 : 1;
            }
        }
        foreach(var num in nums.Values){
            //本質(ノードの個数が色によって違えばtrue)
            if(num != nums[nums.Keys.First()]) return true;
        }
        return false;
    }
}
