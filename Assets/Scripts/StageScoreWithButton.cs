using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class StageScoreWithButton : MonoBehaviour
{
    [field: SerializeField] [field: LabelText("Play Button")]
    public Button PlayButton{ get; private set; }

    [SerializeField] Text indexText;
    [SerializeField] Text scoreText;

    public void Init(StageScoreHolder score, int index){
        scoreText.text = score.bestScore.ToString();
        indexText.text = (index + 1).ToString();
    }
}
