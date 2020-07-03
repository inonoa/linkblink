using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeLight : MonoBehaviour, INodeLight
{
    [SerializeField] Shader shaderOpaque;
    [SerializeField] Shader shaderTransparent;
    [SerializeField] bool alwaysTransparent = false;
    Material mat;
    float defaultEmission;
    const string _Emit = "_Emit";
    const string _Alpha = "_Alpha";

    IEnumerator FadeIn(){
        mat.shader = shaderTransparent;
        mat.ChangeRenderMode(RenderMode.Transparent);

        mat.SetFloat(_Alpha, 0);

        float time = 0;
        while((time += Time.deltaTime) < 0.5f){
            mat.SetFloat(_Alpha, mat.GetFloat(_Alpha) + Time.deltaTime * 2);
            yield return null;
        }

        mat.SetFloat(_Alpha, 1);

        yield return null;

        mat.shader = shaderOpaque;
        mat.ChangeRenderMode(RenderMode.Opaque);

        if(alwaysTransparent){
            mat.renderQueue = 3000;
        }

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

        float time = 0;
        while((time += Time.deltaTime) < 1){
            mat.SetFloat(_Alpha, mat.GetFloat(_Alpha) - Time.deltaTime);
            yield return null;
        }

        mat.SetFloat(_Alpha, 0);

        yield return null;

        Destroy(gameObject);
    }
    [SerializeField] float lightRate = 2;
    [SerializeField] float lightSec = 0.3f;

    IEnumerator lightCoroutine;
    IEnumerator LightCor(){
        float emit;
        while((emit = mat.GetFloat(_Emit)) <= defaultEmission * lightRate){
            mat.SetFloat(_Emit,
                emit + lightRate * (Time.deltaTime / lightSec) * defaultEmission
            );
            yield return null;
        }
    }

    public void Light(){
        if(lightCoroutine != null) StopCoroutine(lightCoroutine);
        if(unlightCoroutine != null) StopCoroutine(unlightCoroutine);
        lightCoroutine = LightCor();
        StartCoroutine(lightCoroutine);
    }

    IEnumerator unlightCoroutine;
    IEnumerator UnLightCor(){
        float emit;
        while((emit = mat.GetFloat(_Emit)) >= defaultEmission){
            mat.SetFloat(_Emit,
                emit - lightRate * (Time.deltaTime / lightSec) * defaultEmission
            );
            yield return null;
        }
    }

    public void UnLight(){
        if(lightCoroutine != null) StopCoroutine(lightCoroutine);
        if(unlightCoroutine != null) StopCoroutine(unlightCoroutine);
        unlightCoroutine = UnLightCor();
        StartCoroutine(unlightCoroutine);
    }

    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        defaultEmission = mat.GetFloat(_Emit);
        StartCoroutine(FadeIn());
    }
}
