using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage
{
    public readonly StageData data;
    public readonly StageScoreHolder scoreHolder;
    public readonly string name;
    public readonly Sequence sequence;
    public Stage(StageData data, StageScoreHolder scoreHolder, string name, Sequence sequence){
        (this.data, this.scoreHolder, this.name, this.sequence) = (data, scoreHolder, name, sequence);
    }
}
