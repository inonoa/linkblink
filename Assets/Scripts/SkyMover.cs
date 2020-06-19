using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SkyMover : MonoBehaviour
{
    [SerializeField] float lightSecs = 1;
    [SerializeField] GameObject moon;

    float moonEmit = 0;
    [SerializeField] Color moonEmitColor = new Color(0.2f, 0.5f, 0.5f, 1);
    [SerializeField] float moonEmitSec = 0.5f;
    Color moonEmitColorDefault;
    void Start() => moonEmitColorDefault = moon.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor");

    public void Light(){

        Material moonMat = moon.GetComponent<MeshRenderer>().material;

        Tween moonTween = DOTween.To(
            () => moonEmit,
            em => {
                moonEmit = em;
                moonMat.SetColor("_EmissionColor", moonEmitColorDefault + em * moonEmitColor);
            },
            1,
            moonEmitSec
        )
        .SetEase(Ease.InOutQuint);

        moonTween.onComplete += () => DOVirtual.DelayedCall(0.2f, () => 
        DOTween.To(
            () => moonEmit,
            em => {
                moonEmit = em;
                moonMat.SetColor("_EmissionColor", moonEmitColorDefault + em * moonEmitColor);
            },
            0,
            moonEmitSec
        )
        .SetEase(Ease.InOutQuint)
        );

        DOTween.To(
            () => RenderSettings.skybox.GetFloat("_LightDistance"),
            dist => RenderSettings.skybox.SetFloat("_LightDistance", dist),
            2,
            lightSecs
        )
        .SetEase(Ease.InOutSine)
        .onComplete += () => RenderSettings.skybox.SetFloat("_LightDistance", -0.1f);
    }
}
