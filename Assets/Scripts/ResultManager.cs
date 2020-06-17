using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public abstract class ResultManager : MonoBehaviour
{
    [SerializeField] protected TotalScoreUI totalScoreUI;
    [SerializeField] protected Image nextButtonImage;
    [SerializeField] protected Tweeter tweeter;
    [SerializeField] protected SoundAndVolume awakeSound;

    protected IEnumerator NextAnim(Material mat){
        float offset = 0;
        while(true){
            mat.SetFloat("Offset", (offset += 0.3f * Time.deltaTime));

            yield return null;
        }
    }

    public abstract void OnNextButtonClick();

    public abstract void OnTweetButtonClick();
}
