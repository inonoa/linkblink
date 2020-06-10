using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Game
{
    public GameData Data{ get; private set; }
    Sequence[] _Sequences;
    public IReadOnlyList<Sequence> Sequences => _Sequences;

    public Game(GameData data){
        Data = data;
        _Sequences = new Sequence[data.Sequences.Count];
        for(int i = 0; i < _Sequences.Length; i++){
            _Sequences[i] = new Sequence(data.Sequences[i]);
        }
        PlayfabAccesssor.Instance.RequestGetData<SequenceScoreHolder>(
            _Sequences.Select(seq => seq.Data.Name + suffix).ToArray(),
            dataDict => {
                foreach(Sequence seq in Sequences){
                    if(dataDict.ContainsKey(seq.Data.Name + suffix)){
                        seq.Scores.InjectBestScores(dataDict[seq.Data.Name + suffix]);
                    }
                }
            }
        );
    }
    const string suffix = "BestScore";
}
