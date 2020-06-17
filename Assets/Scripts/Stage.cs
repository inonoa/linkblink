using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage
{
    public readonly StageData data;
    public readonly StageScoreHolder scoreHolder;
    public readonly string name;
    public Stage(StageData data, StageScoreHolder scoreHolder, string name){
        (this.data, this.scoreHolder, this.name) = (data, scoreHolder, name);
    }
}
