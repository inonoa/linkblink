using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;

public class RankingViewManager : MonoBehaviour
{
    [SerializeField] Transform scrollViewContent;
    [SerializeField] RankDataInjector rankNodePrefab;
    [SerializeField] InputField nameInputField;
    [SerializeField] Button sendButton;
    [SerializeField] Text scoreText;
    [SerializeField] Text onlineBestScoreText;

    CanvasGroup canvasGroup;

    PlayfabAccesssor accesssor;
    [SerializeField][ReadOnly] bool scoreSent = false;
    [SerializeField][ReadOnly] bool bestScore = true;
    int score2send;
    string sequenceName;
    RankData myData;

    SortedList<RankData, object> datas = new SortedList<RankData, object>();

    void Start()
    {

    }

    public void NewScore(){
        scoreSent = false;
        bestScore = true;
    }

    public void Init(string sequenceName, int score){
        this.sequenceName = sequenceName;
        score2send = score;

        foreach(Transform elm in scrollViewContent){
            Destroy(elm.gameObject);
        }
        datas.Clear();

        if(accesssor == null) accesssor = PlayfabAccesssor.Instance;
        accesssor.RequestGetRanking(sequenceName, ranking => {
            ranking.ForEach(data => {
                var rData = new RankData(data.DisplayName ?? "", data.StatValue);
                datas.Add(rData, null);
                if(data.PlayFabId == accesssor.ID){
                    myData = rData;
                    onlineBestScoreText.text = data.StatValue.ToString();
                    if(score2send <= data.StatValue) bestScore = false;
                }
            });
            DrawRanking();
            canvasGroup.DOFade(1, 0.5f);
            transform.DOLocalMoveY(0, 0.5f).SetEase(Ease.OutQuint);
        });

        gameObject.SetActive(true);
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        transform.localPosition = new Vector3(0, -100, 0);

        scoreText.text = score.ToString();
    }

    public void OnCancelButtonClick(){
        foreach(Transform elm in scrollViewContent){
            Destroy(elm.gameObject);
        }

        canvasGroup.DOFade(0, 0.5f);
        transform.DOLocalMoveY(-100, 0.5f).SetEase(Ease.InQuint);
        DOVirtual.DelayedCall(0.5f, () => gameObject.SetActive(false));
    }
    

    void DrawRanking(){
        int index = 0;
        int rank = 1;
        RankData last = null;
        for(int i = datas.Count - 1; i > -1; i--){
            if(last == null || datas.Keys[i].score < last.score) rank = index + 1;
            Instantiate(rankNodePrefab, scrollViewContent).Init(rank, datas.Keys[i]);

            last = datas.Keys[i];
            index ++;
        }
    }
    public void OnSendButtonClick(){
        scoreSent = true;

        string name = nameInputField.text;
        AddData(new RankData(name, score2send));
    }

    void AddData(RankData addedData){

        if(myData != null){
            datas.Remove(myData);
        }
        myData = addedData;
        datas.Add(addedData, null);
        accesssor
            .RequestSetDisplayName(addedData.name)
            .RequestSendScoreToRanking(addedData.score, sequenceName);

        foreach(Transform elm in scrollViewContent){
            Destroy(elm.gameObject);
        }

        DrawRanking();
    }

    void Update(){
        nameInputField.interactable = !scoreSent && bestScore;
        sendButton.interactable = (nameInputField.text.Length > 2 && nameInputField.text.Length < 26 && !scoreSent && bestScore);
    }
}
