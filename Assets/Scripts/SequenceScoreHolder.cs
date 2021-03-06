﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[Serializable]
public class SequenceScoreHolder
{
    public SequenceScoreHolder(int numStages){
        _Scores = new StageScoreHolder[numStages];

        for(int i = 0; i < numStages; i++){
            _Scores[i] = new StageScoreHolder();
        }
    }

    public void InjectBestScores(SequenceScoreHolder bestHolder){
        for(int i = 0; i < bestHolder.Scores.Count; i++){
            Scores[i].bestScore = Mathf.Max(Scores[i].bestScore, bestHolder.Scores[i].bestScore);
        }
        _BestScoreSum = Mathf.Max(BestScoreSum, bestHolder.BestScoreSum);
    }

    public bool RegisterScore(int stageIdx, int score){
        _Scores[stageIdx].score = score;
        return score > _Scores[stageIdx].bestScore;
    }

    public void ApplyBestScores(){
        for(int i = 0; i < _Scores.Length; i++){
            if(_Scores[i].score > _Scores[i].bestScore) Scores[i].bestScore = _Scores[i].score;
        }
        if(ScoreSum > _BestScoreSum) _BestScoreSum = ScoreSum;
    }

    [SerializeField] StageScoreHolder[] _Scores;
    public IReadOnlyList<StageScoreHolder> Scores => _Scores;

    public int ScoreSum => _Scores.Sum(sHolder => sHolder.score);
    [SerializeField] int _BestScoreSum = 0;
    public int BestScoreSum => _BestScoreSum;


    public SequenceScoreHolder Clone(){
        var copy = new SequenceScoreHolder(this._Scores.Count());
        
        copy._Scores.ForEach((i, holder) => {
            holder.score = this.Scores[i].score;
            holder.bestScore = this.Scores[i].bestScore;
        });
        copy._BestScoreSum = this.BestScoreSum;

        return copy;
    }
}

public static class ArrayForEach{

    public static void ForEach<T>(this T[] arr, Action<T> action){
        foreach(T elm in arr){
            action.Invoke(elm);
        }
    }

    public static void ForEach<T>(this T[] arr, Action<int, T> action){
        for(int i = 0; i < arr.Length; i++){
            action.Invoke(i, arr[i]);
        }
    }
}
