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
        radius = transform.localScale.x / 4;
    }

    public void SetRadius(float rad){
        radius = rad;
        transform.localScale = new Vector3(rad * 4, rad * 4, rad * 4);
    }

    public void Explode(NodeMover[] nodes){

        foreach(NodeMover node in nodes){
            if(Vector3.Distance(transform.position, node.transform.position) < radius){
                node.DieSelf();
            }
        }
    }

    public void LitNearNodes(NodeMover[] nodes){
        foreach(NodeMover node in nodes){
            if(Vector3.Distance(transform.position, node.transform.position) < radius){
                //光らせたい
                node.LightBy(this);
            }
        }
    }
    public void UnlitNearNodes(NodeMover[] nodes){
        foreach(NodeMover node in nodes){
            if(Vector3.Distance(transform.position, node.transform.position) < radius){
                //光らせたい
                node.UnLightBy(this);
            }
        }
    }
}
