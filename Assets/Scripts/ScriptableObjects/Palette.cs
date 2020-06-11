using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Palette", menuName = "ScriptableObjects/Palette", order = 11)]
public class Palette : ScriptableObject
{
    [SerializeField] TypeColors[] colors;
    public TypeColors Find(NodeType type){
        foreach(TypeColors col in colors){
            if(col.Type == type) return col;
        }
        return null;
    }

    public static Palette Instance{ get; private set; }
    public Palette() => Instance = this;
}

[Serializable]
public class TypeColors{
    
    [SerializeField] NodeType _Type;
    public NodeType Type => _Type;
    [SerializeField] Color _EditorColor = new Color(1,1,1,1);
    public Color EditorColor => _EditorColor;
}
