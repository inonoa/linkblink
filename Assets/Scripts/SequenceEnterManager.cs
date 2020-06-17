using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SequenceEnterManager : MonoBehaviour
{
    [SerializeField] StageWatcher stageWatcher;
    [SerializeField] Text sequenceNameText;
    [SerializeField] StageScoreWithButton[] stagesUIs;
    [SerializeField] SequenceSelectScene sequenceSelectScene;
    [SerializeField] Text bestScoreText;
    [SerializeField] RankingViewManager rankingViewManager;

    CanvasGroup canvasGroup;

    Sequence currentSequence;

    public void Init(Sequence seq){
        currentSequence = seq;
        Show();
    }

    public void Show(){

        gameObject.SetActive(true);
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, 0.5f);

        for(int i = 0; i < currentSequence.Stages.Count; i++){
            stagesUIs[i].Init(currentSequence.Stages[i].scoreHolder, i);
            int i_ = i;
            stagesUIs[i].PlayButton.onClick.RemoveAllListeners();
            stagesUIs[i].PlayButton.onClick.AddListener(() => OnStagePlayButtonPushed(currentSequence.Stages[i_]));
        }
        sequenceNameText.text = currentSequence.Data.Name;
        bestScoreText.text = currentSequence.Scores.BestScoreSum.ToString();

        rankingViewManager.LoadRanking(currentSequence.Data.Name, currentSequence.Scores.BestScoreSum);
    }

    public void OnPlayButtonPushed(){
        stageWatcher.Init(currentSequence);
        FadeOut();
    }

    void OnStagePlayButtonPushed(Stage stage){
        stageWatcher.Init(stage);
        FadeOut();
    }

    void FadeOut(){
        canvasGroup.DOFade(0, 0.5f)
            .onComplete += () => gameObject.SetActive(false);
    }

    public void OnRankingButtonPushed(){
        rankingViewManager.Show();
    }

    public void OnReturnButtonPushed(){
        sequenceSelectScene.ReStart();
        FadeOut();
    }
}
