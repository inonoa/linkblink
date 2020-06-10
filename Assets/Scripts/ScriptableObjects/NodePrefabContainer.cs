using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[CreateAssetMenu(fileName = "NodePrefabs", menuName = "ScriptableObjects/NodePrefabContainer", order = 10)]
public class NodePrefabContainer : ScriptableObject
{
    [SerializeField] TypeNodePair[] prefabs;
    public NodeMover this[NodeType type]{
        get{
            foreach(var kvp in prefabs){
                if(kvp.Key == type) return kvp.Value;
            }
            return null;
        }
    }

    public NodePrefabContainer() => Instance = this;
    static public NodePrefabContainer Instance{ get; private set; }
}

[Serializable]
public class TypeNodePair{
    [SerializeField] NodeType _Key;
    public NodeType Key => _Key;
    [SerializeField] NodeMover _Value;
    public NodeMover Value => _Value;
}
