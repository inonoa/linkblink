using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;

[Serializable]
public class Sequence
{
    [field:SerializeField][field:ReadOnly][field:LabelText("Data")]
    public SequenceData Data{ get; private set; }
    [SerializeField] SequenceScoreHolder _Scores;
    public SequenceScoreHolder Scores => _Scores;
    public bool playedYet = false;

    Stage[] _Stages;
    public IReadOnlyList<Stage> Stages => _Stages;
    
    public Sequence(SequenceData data){
        Data = data;
        _Scores = new SequenceScoreHolder(Data.Stages.Count);

        _Stages = new Stage[data.Stages.Count];
        for(int i = 0; i < _Stages.Length; i++){
            _Stages[i] = new Stage(Data.Stages[i], _Scores.Scores[i], data.Name + " stage " + (i+1), this);
        }
    }
}
