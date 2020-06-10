using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Beam : MonoBehaviour, IVanish
{
    [SerializeField] Shader oneOrTwoColorShader;
    [SerializeField] Shader allColorShader;

    public LineRenderer LineRenderer{
        get{
            if(_LineRenderer == null) _LineRenderer = GetComponent<LineRenderer>();
            return _LineRenderer;
        }
    }
    LineRenderer _LineRenderer;

    [SerializeField] Color cyanEmit;
    [SerializeField] Color magentaEmit;
    [SerializeField] Color yellowEmit;
    [SerializeField] Color greenEmit;
    [SerializeField] Color cyanMagentaEmit;
    [SerializeField] Color allColorEmit;

    public void SetColor(NodeType type){
        switch(type){

            case NodeType.Cyan: {
                LineRenderer.material.shader = oneOrTwoColorShader;
                LineRenderer.material.SetColor("_TintColor", new Color(0, 0.8f, 0.8f, 1f));
                LineRenderer.material.SetColor("_TintColor2", new Color(0, 0, 0, 0));
                LineRenderer.material.SetColor("_EmitColor", cyanEmit);
                break;
            }
            case NodeType.Magenta: {
                LineRenderer.material.shader = oneOrTwoColorShader;
                LineRenderer.material.SetColor("_TintColor", new Color(0.8f, 0, 0.8f, 1f));
                LineRenderer.material.SetColor("_TintColor2", new Color(0, 0, 0, 0));
                LineRenderer.material.SetColor("_EmitColor", magentaEmit);
                break;
            }
            case NodeType.Yellow: {
                LineRenderer.material.shader = oneOrTwoColorShader;
                LineRenderer.material.SetColor("_TintColor", new Color(0.8f, 0.8f, 0, 1f));
                LineRenderer.material.SetColor("_TintColor2", new Color(0, 0, 0, 0));
                LineRenderer.material.SetColor("_EmitColor", yellowEmit);
                break;
            }
            case NodeType.Green: {
                LineRenderer.material.shader = oneOrTwoColorShader;
                LineRenderer.material.SetColor("_TintColor", new Color(0.25f, 0.8f, 0.25f, 1));
                LineRenderer.material.SetColor("_TintColor2", new Color(0, 0, 0, 0));
                LineRenderer.material.SetColor("_EmitColor", greenEmit);
                break;
            }
            case NodeType.CyanMagenta: {
                LineRenderer.material.shader = oneOrTwoColorShader;
                LineRenderer.material.SetColor("_TintColor", new Color(0, 0.8f, 0.8f, 1));
                LineRenderer.material.SetColor("_TintColor2", new Color(0.8f, 0, 0.8f, 1f));
                LineRenderer.material.SetColor("_EmitColor", cyanMagentaEmit);
                break;
            }
            case NodeType.AllColor: {
                LineRenderer.material.shader = allColorShader;
                LineRenderer.material.SetColor("_EmitColor", allColorEmit);
                break;
            }
        }
        Light();
    }

    void Start(){
        scrollCoroutine = Scroll();
        StartCoroutine(scrollCoroutine);
    }
    IEnumerator Scroll(){
        float scrollX = 0;
        while(true){
            scrollX += Time.deltaTime / 2;
            scrollX %= 1;
            LineRenderer.material.SetFloat("_ScrollX", scrollX);

            yield return null;
        }
    }


    static readonly string _Emit = "_Emit";
    Tween lightTween;
    [SerializeField] float lightSec = 0.7f;

    [Button]
    public void Light(){
        var lightSeq = DOTween.Sequence();
        lightSeq.Append(
            DOTween.To(
                () => LineRenderer.material.GetFloat(_Emit),
                em => LineRenderer.material.SetFloat(_Emit, em),
                1,
                lightSec / 2
            )
        );
        lightSeq.Append(
            DOTween.To(
                () => LineRenderer.material.GetFloat(_Emit),
                em => LineRenderer.material.SetFloat(_Emit, em),
                0,
                lightSec / 2
            )
        );
        lightTween = lightSeq;
    }

    IEnumerator scrollCoroutine;
    public void Vanish(){
        if(scrollCoroutine != null){
            StopCoroutine(scrollCoroutine);
        }
        scrollCoroutine = SlowScroll();
        StartCoroutine(scrollCoroutine);
        StartCoroutine(VanishCoroutine());
    }

    IEnumerator SlowScroll(){
        float scrollX = LineRenderer.material.GetFloat("_ScrollX");
        while(true){
            scrollX += Time.deltaTime / 5;
            scrollX %= 1;
            LineRenderer.material.SetFloat("_ScrollX", scrollX);

            yield return null;
        }
    }

    IEnumerator VanishCoroutine(){
        Material mat = LineRenderer.material;
        float alpha = mat.GetFloat("_Alpha");

        while((alpha -= Time.deltaTime) > 0){
            mat.SetFloat("_Alpha", alpha);
            yield return null;
        }

        StopCoroutine(scrollCoroutine);
        Destroy(gameObject);
    }
}
