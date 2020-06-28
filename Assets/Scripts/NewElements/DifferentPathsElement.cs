using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "DiferrentPathsElement", menuName = "ScriptableObjects/DifferentPathsElement", order = 5)]
public class DifferentPathsElement : NewElement
{

    protected override bool ExistIn(StageData data){
        return data.NewElementTags.Contains(ElementTag.DifferentPath);
    }
}
