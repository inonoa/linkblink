using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RankingViewManager : MonoBehaviour
{
    [SerializeField] Transform scrollViewContent;
    [SerializeField] RankDataInjector rankNodePrefab;
    [SerializeField] Text onlineBestScoreText;

    CanvasGroup canvasGroup;

    List<RankData> datas = new List<RankData>();
    bool dataLoaded = false;

    public void LoadRanking(string sequenceName, int bestScore){
        dataLoaded = false;

        foreach(Transform elm in scrollViewContent){
            Destroy(elm.gameObject);
        }

        datas.Clear();
        PlayfabAccesssor.Instance.RequestGetRanking(sequenceName, ranking => {
            ranking.ForEach(data => {
                var rData = new RankData(data.DisplayName ?? "", data.StatValue);
                datas.Add(rData);
            });
            dataLoaded = true;
        });

        onlineBestScoreText.text = bestScore.ToString();
    }

    void DrawRanking(){
        int index = 0;
        int rank = 1;
        RankData last = null;
        for(int i = 0; i < datas.Count; i++){
            if(last == null || datas[i].score < last.score) rank = index + 1;
            Instantiate(rankNodePrefab, scrollViewContent).Init(rank, datas[i]);

            last = datas[i];
            index ++;
        }
    }

    public void Show(){

        gameObject.SetActive(true);
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        transform.localPosition = new Vector3(0, -100, 0);

        StartCoroutine(ShowCoroutine());

        IEnumerator ShowCoroutine(){

            yield return new WaitUntil(() => dataLoaded);

            DrawRanking();
            canvasGroup.DOFade(1, 0.5f);
            transform.DOLocalMoveY(0, 0.5f).SetEase(Ease.OutQuint);
        }
    }

    public void OnCancelButtonClick(){
        foreach(Transform elm in scrollViewContent){
            Destroy(elm.gameObject);
        }

        canvasGroup.DOFade(0, 0.5f);
        transform.DOLocalMoveY(-100, 0.5f).SetEase(Ease.InQuint);
        DOVirtual.DelayedCall(0.5f, () => gameObject.SetActive(false));
    }


    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
}
