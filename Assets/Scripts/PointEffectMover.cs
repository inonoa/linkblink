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
