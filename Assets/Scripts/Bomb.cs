using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Bomb : MonoBehaviour
{
    [SerializeField] NodeMover[] nodesTmp;
    [Button] void ExplodeTest() => Explode(nodesTmp);

    float radius;

    void Start()
    {
        radius = transform.localScale.x / 2;
    }

    public void Explode(NodeMover[] nodes){

        foreach(NodeMover node in nodes){
            if(Vector3.Distance(transform.position, node.transform.position) < radius){
                //Boardに通知しないといけなそう
                node.DieSelf();
            }
        }
    }
}
