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

    CanvasGroup canvasGroup;

    Sequence currentSequence;

    public void Init(Sequence seq){

        gameObject.SetActive(true);
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, 0.5f);

        currentSequence = seq;
        for(int i = 0; i < seq.Stages.Count; i++){
            stagesUIs[i].Init(seq.Stages[i].scoreHolder, i);
            int i_ = i;
            stagesUIs[i].PlayButton.onClick.AddListener(() => OnStagePlayButtonPushed(seq.Stages[i_]));
        }
        sequenceNameText.text = seq.Data.Name;
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
        //
    }

    public void OnReturnButtonPushed(){
        sequenceSelectScene.ReStart();
        FadeOut();
    }
}
