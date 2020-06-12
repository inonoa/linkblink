using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TypeData", menuName = "ScriptableObjects/TypeData", order = 2)]
public class TypeDataHolder : ScriptableObject{
    [SerializeField] NodeTypeData[] _Data;

    public NodeTypeData this[NodeType type]{
        get{
            foreach(NodeTypeData datum in _Data){
                if(datum.Type == type) return datum;
            }
            return null;
        }
    }


    public static TypeDataHolder Instance{ get; private set; }
    public TypeDataHolder(){
        Instance = this;
    }
}