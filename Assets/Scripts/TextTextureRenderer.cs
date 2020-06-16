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

    [SerializeField] [ReadOnly] Texture2D lastGeneratedTexture;

    public Texture2D Render(string text){
        this.text.text = text;

        RenderTexture rt = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
        rt.Create();
        Camera main = Camera.main;
        Camera.SetupCurrent(camera);
        camera.gameObject.SetActive(true);
        camera.targetTexture = rt;
        camera.Render();
        camera.gameObject.SetActive(false);

        RenderTexture.active = rt;
        Texture2D tex = new Texture2D(256, 256, TextureFormat.ARGB32, false, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();
        RenderTexture.active = null;

        Camera.SetupCurrent(main);

        lastGeneratedTexture = tex;

        return tex;
    }

    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
