using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NodeLightWithBomb : MonoBehaviour, INodeLight
{
    [SerializeField] MeshRenderer bomb;
    Material bombMat;
    float bombDefaultLight;
    const string _Light = "_Light";
    [SerializeField] float bombLightRate = 2;
    [SerializeField] float bombLightSec = 0.3f;
    Tween currentTween;


    [SerializeField] NodeLightCore core;

    void Start()
    {
        core.Init(GetComponent<MeshRenderer>().material, this);
        bombMat = bomb.material;
        bombDefaultLight = bombMat.GetFloat(_Light);
        BombFadeIn();
    }
    public void BombFadeIn(){

        bombMat.SetFloat(_Light, 0);

        var seq = DOTween.Sequence();
        seq.Append(LightTo(
            bombDefaultLight,                 0.5f));
        seq.Append(LightTo(
            bombDefaultLight * bombLightRate, bombLightSec));
        seq.Append(LightTo(
            bombDefaultLight,                 bombLightSec));
        currentTween = seq;
    }

    public void Light(){
        core.Light();
        
        currentTween?.Kill();
        currentTween = LightTo(bombDefaultLight * bombLightRate, bombLightSec);
    }

    public void UnLight(){
        core.UnLight();
        currentTween?.Kill();
        currentTween = LightTo(bombDefaultLight, bombLightSec);
    }
    public void Vanish(){
        GetComponent<Collider>().enabled = false;
        GetComponent<Rotator>().Stop();
        core.Vanish();

        bombMat.SetFloat(_Light, bombDefaultLight);
        currentTween?.Kill();
        currentTween = LightTo(0, 1);
    }

    float GetLight() => bombMat.GetFloat(_Light);
    void SetLight(float val) => bombMat.SetFloat(_Light, val);
    Tween LightTo(float dst, float duration){
        return DOTween.To(GetLight, SetLight, dst, duration);
    }
}
