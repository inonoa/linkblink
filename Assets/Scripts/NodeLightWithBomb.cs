﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeLightWithBomb : MonoBehaviour, INodeLight
{
    [SerializeField] MeshRenderer bomb;
    Material bombMat;
    float bombDefaultLight;

    [SerializeField] Shader shaderOpaque;
    [SerializeField] Shader shaderTransparent;
    Material mat;
    float defaultEmission;
    const string _Emit = "_Emit";
    const string _Alpha = "_Alpha";
    const string _Light = "_Light";

    IEnumerator FadeIn(){
        mat.shader = shaderTransparent;
        mat.ChangeRenderMode(RenderMode.Transparent);

        mat.SetFloat(_Alpha, 0);
        bombMat.SetFloat(_Light, 0);

        float time = 0;
        while((time += Time.deltaTime) < 0.5f){
            mat.SetFloat(_Alpha, mat.GetFloat(_Alpha) + Time.deltaTime * 2);
            bombMat.SetFloat(_Light, bombMat.GetFloat(_Light) + Time.deltaTime);
            yield return null;
        }

        mat.SetFloat(_Alpha, 1);
        bombMat.SetFloat(_Light, 0.5f);

        yield return null;

        mat.shader = shaderOpaque;
        mat.ChangeRenderMode(RenderMode.Opaque);

        Light();
        yield return new WaitForSeconds(0.7f); //lightが終わるまで+ちょっと
        UnLight();
    }

    public void Vanish(){
        GetComponent<Collider>().enabled = false;

        GetComponent<Rotator>().Stop();
        StartCoroutine(VanishCoroutine());
    }

    IEnumerator VanishCoroutine(){

        mat.shader = shaderTransparent;
        mat.ChangeRenderMode(RenderMode.Transparent);

        mat.SetFloat(_Emit, 0);
        bombMat.SetFloat(_Light, bombDefaultLight);

        float time = 0;
        while((time += Time.deltaTime) < 1){
            mat.SetFloat(_Alpha, mat.GetFloat(_Alpha) - Time.deltaTime);
            bombMat.SetFloat(_Light, bombMat.GetFloat(_Light) - Time.deltaTime * bombDefaultLight);
            yield return null;
        }

        mat.SetFloat(_Alpha, 0);
        bombMat.SetFloat(_Light, 0);

        yield return null;

        Destroy(gameObject);
    }
    [SerializeField] float lightRate = 2;
    [SerializeField] float bombLightRate = 2;
    [SerializeField] float lightSec = 0.3f;

    IEnumerator lightCoroutine;
    IEnumerator LightCor(){
        float emitDst = defaultEmission * lightRate;
        float bombLightDst = bombDefaultLight * bombLightRate;

        float time = 0;
        while((time += Time.deltaTime) <= lightSec){
            mat.SetFloat(_Emit, Mathf.Lerp(defaultEmission, emitDst, time / lightSec));
            bombMat.SetFloat(_Light, Mathf.Lerp(bombDefaultLight, bombLightDst, time / lightSec));
            yield return null;
        }

        mat.SetFloat(_Emit, emitDst);
        bombMat.SetFloat(_Light, bombLightDst);
    }

    public void Light(){
        if(lightCoroutine != null) StopCoroutine(lightCoroutine);
        if(unlightCoroutine != null) StopCoroutine(unlightCoroutine);
        lightCoroutine = LightCor();
        StartCoroutine(lightCoroutine);
    }

    IEnumerator unlightCoroutine;
    IEnumerator UnLightCor(){
        float emitStart = defaultEmission * lightRate;
        float bombLightStart = bombDefaultLight * bombLightRate;

        float time = lightSec;
        while((time -= Time.deltaTime) >= 0){
            mat.SetFloat(_Emit, Mathf.Lerp(defaultEmission, emitStart, time / lightSec));
            bombMat.SetFloat(_Light, Mathf.Lerp(bombDefaultLight, bombLightStart, time / lightSec));
            yield return null;
        }

        mat.SetFloat(_Emit, defaultEmission);
        bombMat.SetFloat(_Light, bombDefaultLight);
    }

    public void UnLight(){
        if(lightCoroutine != null) StopCoroutine(lightCoroutine);
        if(unlightCoroutine != null) StopCoroutine(unlightCoroutine);
        unlightCoroutine = UnLightCor();
        StartCoroutine(unlightCoroutine);
    }

    void Start()
    {
        bombMat = bomb.material;
        bombDefaultLight = bombMat.GetFloat(_Light);
        mat = GetComponent<MeshRenderer>().material;
        defaultEmission = mat.GetFloat(_Emit);
        StartCoroutine(FadeIn());
    }
}
