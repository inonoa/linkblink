using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage
{
    public readonly StageData data;
    public readonly StageScoreHolder scoreHolder;

    public Stage(StageData data, StageScoreHolder scoreHolder){
        (this.data, this.scoreHolder) = (data, scoreHolder);
    }
}
