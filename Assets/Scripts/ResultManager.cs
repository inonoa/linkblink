using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [SerializeField] Text sequenceNameText;
    [SerializeField] TotalScoreUI totalScoreUI;
    [SerializeField] Image nextButtonImage;
    [SerializeField] RecordUI[] stageScoreUIs;
    [SerializeField] SequenceSelectManager sequenceSelectManager;
    [SerializeField] RankingViewManager rankingViewManager;
    [SerializeField] Tweeter tweeter;
    [SerializeField] SoundAndVolume awakeSound;

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

    IEnumerator NextAnim(Material mat){
        float offset = 0;
        while(true){
            mat.SetFloat("Offset", (offset += 0.3f * Time.deltaTime));

            yield return null;
        }
    }

    //以下はUnity側でボタンと結びついております

    public void OnNextButtonClick(){
        sequence.Scores.ApplyBestScores();
        PlayfabAccesssor.Instance.RequestSendData(sequence.Data.Name + DebugParameters.Instance.BestScoreSuffix, sequence.Scores);
        var group = GetComponent<CanvasGroup>();
        group.DOFade(0, 1f).SetEase(Ease.OutQuint);
        DOVirtual.DelayedCall(1f, () => {
            gameObject.SetActive(false);
            sequenceSelectManager.Init();
        });
    }

    public void OnTweetButtonClick(){
        tweeter.Tweet(sequence.Scores.ScoreSum, sequence.Data.Name);
    }

    public void OnRankButtonClick(){
        rankingViewManager.Init(sequence.Data.Name, sequence.Scores.ScoreSum);
    }
}
