using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PointEffectMover : MonoBehaviour
{
    [SerializeField] float moveDistance = 1;
    [SerializeField] float moveDuration = 2;
    [SerializeField] float fadeInDuration = 0.2f;
    [SerializeField] float fadeOutDelay = 1;
    [SerializeField] float fadeOutDuration = 1;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X)) Init(1770, NodeColor.Cyan);
        if(Input.GetKeyDown(KeyCode.C)) Init(1770, NodeColor.Magenta);
        if(Input.GetKeyDown(KeyCode.V)) Init(1770, NodeColor.Yellow);
        if(Input.GetKeyDown(KeyCode.B)) Init(1770, NodeColor.Green);
    }

    void Start(){
        var text = GetComponent<TextMesh>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
    }


    public void Init(int point, NodeColor nodeColor){

        //移動
        transform.DOMoveY(moveDistance, moveDuration)
            .SetRelative()
            .SetEase(Ease.OutQuint);
        
        //色の初期化
        var text = GetComponent<TextMesh>();
        var color = TypeDataHolder.Instance[nodeColor.ToType()].PointTextColor;
        color.a = 0;
        text.color = color;
        //フェードイン
        DOTween.To(
            () => text.color.a,
            a => text.color = new Color(text.color.r, text.color.g, text.color.b, a),
            1,
            fadeInDuration
        );
        //フェードアウト
        DOVirtual.DelayedCall(
            fadeOutDelay,
            () => DOTween.To(
                () => text.color.a,
                a => text.color = new Color(text.color.r, text.color.g, text.color.b, a),
                0,
                fadeOutDuration
            )
            .onComplete += () => Destroy(gameObject)
        );
    }
}
