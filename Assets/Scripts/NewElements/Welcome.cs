using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WelcomeElement", menuName = "ScriptableObjects/WelcomeElement", order = 5)]
public class Welcome : NewElement
{

    protected override bool ExistIn(StageData data){
        return true;
    }
}
