using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using System.Linq;


[CreateAssetMenu(fileName = "Stage", menuName = "ScriptableObjects/StageData", order = 1)]
public class StageData : ScriptableObject{
    [SerializeField, TextArea] string memo;

    enum Difficulty{
        None, ForTutorial, Easy, Normal, Hard, Lunatic
    }
    [SerializeField] Difficulty difficulty;
    [SerializeField] Vector2 _DistanceUnit = new Vector2();
    public Vector2 DistanceUnit => _DistanceUnit;
    [SerializeField] [ListDrawerSettings( Expanded = true )] NodeRow[] _Rows;
    public IReadOnlyList<NodeRow> Rows => _Rows;

    [SerializeField] NodeType[,] nodesTest;
    [SerializeField] NodeType[][] nodesTest2;

    [Button]
    void GoToNormal(){
        if(!GameData.Instance.Normal.Stages.Contains(this)) GameData.Instance.Normal.Add(this);
    }
    [Button]
    void GoToHard(){
        if(!GameData.Instance.Hard.Stages.Contains(this)) GameData.Instance.Hard.Add(this);
    }
    [Button]
    void CopyArray(StageData src){
        _Rows = new NodeRow[src.Rows.Count];
        for(int i = 0; i < src.Rows.Count; i++){
            _Rows[i] = new NodeRow();
            _Rows[i].CopyFrom(src.Rows[i]);
        }
    }
}

[Serializable]
public class NodeRow{
    [SerializeField] [ListDrawerSettings( Expanded = true )] NodeType[] _Nodes;
    public IReadOnlyList<NodeType> Nodes => _Nodes;
    public void CopyFrom(NodeRow src){
        this._Nodes = src.Nodes.Select(nd => nd).ToArray();
    }

    [Button]
    void Fill(NodeType type){
        for(int i = 0; i < Nodes.Count; i++){
            _Nodes[i] = type;
        }
    }
}
