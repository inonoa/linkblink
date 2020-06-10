using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BGMFadeIn : MonoBehaviour
{
    [SerializeField, Range(0, 1)] float maxVolume = 0.15f;
    [SerializeField] float fadeInSecs = 3;

    new AudioSource audio;
    float BGMLength;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        BGMLength = audio.clip.length;
        audio.volume = 0;
        audio.DOFade(maxVolume, fadeInSecs);
    }


    void Update()
    {
        if(audio.time > BGMLength * 1.5f / 2){
            //print("looop!!");
            audio.time -= BGMLength / 2;
        }
    }
}
