using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;


public class TextTextureRenderer : MonoBehaviour
{
    [SerializeField] new Camera camera;
    [SerializeField] Text text;

    [SerializeField] [ReadOnly] Texture lastGeneratedTexture;

    public Texture Render(string text){
        this.text.text = text;

        RenderTexture rt = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
        rt.Create();
        camera.gameObject.SetActive(true);
        camera.targetTexture = rt;
        camera.Render();
        camera.gameObject.SetActive(false);

        lastGeneratedTexture = rt;

        return rt;
    }

    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
