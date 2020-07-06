using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "YubakuElement", menuName = "ScriptableObjects/YubakuElement", order = 7)]
public class YubakuElement : NewElement
{

    protected override bool ExistIn(StageData data){
        return data.NewElementTags.Contains(ElementTag.Yubaku);
    }
}
