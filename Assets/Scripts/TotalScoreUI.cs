using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TotalScoreUI : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] Text bestScoreText;
    [SerializeField] Text highScore;

    public void Init(int score, int bestScore){
        scoreText.text = score.ToString();
        bestScoreText.text = bestScore.ToString();
        var scoreCol = scoreText.color; scoreCol.a = 0; scoreText.color = scoreCol;
        var highScoreCol = highScore.color; highScoreCol.a = 0; highScore.color = highScoreCol;
        DOVirtual.DelayedCall(1.2f, () => {
            scoreText.DOFade(1, 0.5f).SetEase(Ease.OutQuint);
            if(score > bestScore) highScore.DOFade(1, 0.5f).SetEase(Ease.OutQuint);
        });
    }
}
