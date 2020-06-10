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
    public Sequence(SequenceData data){
        Data = data;
        if(Data.InitialScores != null && Data.InitialScores.Count == Data.Stages.Count){
            _Scores = new SequenceScoreHolder(Data.InitialScores.ToArray());
        }else{
            _Scores = new SequenceScoreHolder(new int[Data.Stages.Count]);
        }
    }
}
