using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

public class Bomb : MonoBehaviour
{
    float radius;

    void Start()
    {
        radius = transform.localScale.x / 4;
    }

    public void SetRadius(float rad){
        radius = rad;
        transform.localScale = new Vector3(rad * 4, rad * 4, rad * 4);
    }

    public void Explode(NodeMover[] nodes){

        foreach(NodeMover node in nodes){
            if(Vector3.Distance(transform.position, node.transform.position) < radius){
                DOVirtual.DelayedCall(0.2f, node.DieSelf);
            }
        }
    }

    //このふたつここにあるべきじゃない気がする
    public void LitNearNodes(NodeMover[] nodes){
        foreach(NodeMover node in nodes){
            if(Vector3.Distance(transform.position, node.transform.position) < radius){
                if(blinks.ContainsKey(node)) StopCoroutine(blinks[node]);
                blinks[node] = Blink(node);
                StartCoroutine(blinks[node]);
                node.LightBy(this);
            }
        }
    }
    public void UnlitNearNodes(NodeMover[] nodes){
        foreach(NodeMover node in nodes){
            if(Vector3.Distance(transform.position, node.transform.position) < radius){
                if(blinks.ContainsKey(node)) StopCoroutine(blinks[node]);
                node.UnLightBy(this);
            }
        }
    }

    Dictionary<NodeMover, IEnumerator> blinks = new Dictionary<NodeMover, IEnumerator>();
    IEnumerator Blink(NodeMover node){
        
        while(node.isActiveAndEnabled && node.IsLive){
            node.LightBy(this);
            yield return new WaitForSeconds(0.3f);
            if((!node.isActiveAndEnabled) || (!node.IsLive)) yield break;
            node.UnLightBy(this);
            yield return new WaitForSeconds(0.3f);
        }
    }
}
