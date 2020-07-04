using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeLight : MonoBehaviour, INodeLight
{
    [SerializeField] NodeLightCore core;

    void Start()
    {
        core.Init(GetComponent<MeshRenderer>().material, this);
    }

    public void Light(){
        core.Light();
    }

    public void UnLight(){
        core.UnLight();
    }
    public void Vanish(){
        GetComponent<Collider>().enabled = false;
        GetComponent<Rotator>().Stop();
        core.Vanish();
    }
}
