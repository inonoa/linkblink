using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[Serializable]
public class SequenceScoreHolder
{
    public SequenceScoreHolder(int[] bestScoresOrZeros){
        if(bestScoresOrZeros.Any(sc => sc != 0)){
            _BestScores = bestScoresOrZeros;
            _Scores = new int[bestScoresOrZeros.Length];
        }else{
            _BestScores = new int[bestScoresOrZeros.Length];
            _Scores = new int[bestScoresOrZeros.Length];
        }
    }

    public bool RegisterScore(int stageIdx, int score){
        //ScoreSum -= _Scores[stageIdx];
        _Scores[stageIdx] = score;
        //ScoreSum += score;
        return score > BestScores[stageIdx];
    }

    public void InjectBestScores(SequenceScoreHolder bestHolder){
        for(int i = 0; i < bestHolder.BestScores.Count; i++){
            _BestScores[i] = Mathf.Max(BestScores[i], bestHolder.BestScores[i]);
        }
        _BestScoreSum = Mathf.Max(BestScoreSum, bestHolder.BestScoreSum);
    }

    public void ApplyBestScores(){
        for(int i = 0; i < _Scores.Length; i++){
            if(_BestScores[i] < _Scores[i]) _BestScores[i] = _Scores[i];
        }
        if(ScoreSum > _BestScoreSum) _BestScoreSum = ScoreSum;
    }

    public IReadOnlyList<int> Scores => _Scores;
    public IReadOnlyList<int> BestScores => _BestScores;

    public int ScoreSum{
        get{
            return _Scores.Sum();
        }
    }
    [SerializeField] int _BestScoreSum = 0;
    public int BestScoreSum => _BestScoreSum;

    [SerializeField] int[] _Scores = new int[10];
    [SerializeField] int[] _BestScores = new int[10];
}
