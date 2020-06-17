using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SequenceResultManager : ResultManager
{
    [SerializeField] Text sequenceNameText;
    [SerializeField] RecordUI[] stageScoreUIs;
    [SerializeField] SequenceSelectScene sequenceSelectScene;
    [SerializeField] RankingWritableViewManager rankingViewManager;

    Sequence sequence;

    public void Init(Sequence sequence){
        gameObject.SetActive(true);
        rankingViewManager.NewScore();

        var group = GetComponent<CanvasGroup>();
        group.alpha = 0;
        group.DOFade(1, 1f).SetEase(Ease.OutQuint);
        awakeSound.Play();

        sequenceNameText.text = sequence.Data.Name;

        totalScoreUI.Init(sequence.Scores.ScoreSum, sequence.Scores.BestScoreSum);

        for(int i = 0; i < sequence.Scores.Scores.Count; i++){
            StageScoreHolder stageholder = sequence.Scores.Scores[i];

            stageScoreUIs[i].StageIndexText.text = (i+1).ToString();
            stageScoreUIs[i].ScoreText.text      = stageholder.score.ToString();
            stageScoreUIs[i].BestScoreText.text  = stageholder.bestScore.ToString();
            stageScoreUIs[i].SetIsBestScore(stageholder.score > stageholder.bestScore);
        }

        this.sequence = sequence;

        DOVirtual.DelayedCall(3f, () => {
            if(gameObject.activeInHierarchy) StartCoroutine(NextAnim(nextButtonImage.material));
        });
    }

    //以下はUnity側でボタンと結びついております

    public override void OnNextButtonClick(){

        sequence.Scores.ApplyBestScores();
        sequence.playedYet = true;
        PlayfabAccesssor.Instance.RequestSendData(sequence.Data.Name + DebugParameters.Instance.BestScoreSuffix, sequence.Scores);
        var group = GetComponent<CanvasGroup>();
        group.DOFade(0, 1f).SetEase(Ease.OutQuint);
        DOVirtual.DelayedCall(1f, () => {
            gameObject.SetActive(false);
            sequenceSelectScene.ReStart();
        });
    }

    public void OnRankButtonClick(){
        rankingViewManager.Init(sequence.Data.Name, sequence.Scores.ScoreSum);
    }

    public override void OnTweetButtonClick(){
        tweeter.Tweet(sequence.Scores.ScoreSum, sequence.Data.Name);
    }
}
