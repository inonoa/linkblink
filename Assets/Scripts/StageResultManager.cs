using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StageResultManager : ResultManager
{
    [SerializeField] Text stageNameText;
    [SerializeField] SequenceEnterManager sequenceEnterManager;

    Stage currentStage;

    public void Init(Stage stage){
        currentStage = stage;

        gameObject.SetActive(true);

        var group = GetComponent<CanvasGroup>();
        group.alpha = 0;
        group.DOFade(1, 1f).SetEase(Ease.OutQuint);
        awakeSound.Play();

        stageNameText.text = stage.name;

        totalScoreUI.Init(stage.scoreHolder.score, stage.scoreHolder.bestScore);

        DOVirtual.DelayedCall(3f, () => {
            if(gameObject.activeInHierarchy) StartCoroutine(NextAnim(nextButtonImage.material));
        });
    }

    public override void OnNextButtonClick(){
        currentStage.scoreHolder.bestScore = Mathf.Max(currentStage.scoreHolder.bestScore, currentStage.scoreHolder.score);

        var group = GetComponent<CanvasGroup>();
        group.DOFade(0, 1f).SetEase(Ease.OutQuint);
        DOVirtual.DelayedCall(1f, () => {
            gameObject.SetActive(false);
            sequenceEnterManager.Show();
        });
    }

    public override void OnTweetButtonClick(){
        tweeter.Tweet(currentStage.scoreHolder.score, stageName: currentStage.name);
    }
}
