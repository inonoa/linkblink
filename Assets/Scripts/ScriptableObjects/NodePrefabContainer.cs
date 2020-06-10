using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[CreateAssetMenu(fileName = "NodePrefabs", menuName = "ScriptableObjects/NodePrefabContainer", order = 10)]
public class NodePrefabContainer : SerializedScriptableObject
{
    [SerializeField] Dictionary<NodeType, NodeMover> _Prefabs;
    public IReadOnlyDictionary<NodeType, NodeMover> Prefabs => _Prefabs;

    public NodePrefabContainer() => Instance = this;
    static public NodePrefabContainer Instance{ get; private set; }
}
