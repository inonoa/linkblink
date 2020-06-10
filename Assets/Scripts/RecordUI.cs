using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordUI : MonoBehaviour
{
    [SerializeField] Text _StageIndexText;
    public Text StageIndexText => _StageIndexText;

    [SerializeField] Text _ScoreText;
    public Text ScoreText => _ScoreText;

    [SerializeField] Text _BestScoreText;
    public Text BestScoreText => _BestScoreText;

    [SerializeField] Color bestScoreColor = new Color(0.8f,1,0.5f,1);
    [SerializeField] Color defaultScoreColor = new Color(0.9f,0.9f,0.9f,1);

    public void SetIsBestScore(bool isBestScore){
        _ScoreText.color = isBestScore ? bestScoreColor : defaultScoreColor;
    }
}
