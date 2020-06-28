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
    [SerializeField] ElementTag[] newElementTags;
    public IReadOnlyList<ElementTag> NewElementTags => newElementTags;
    [SerializeField] Vector2 _DistanceUnit = new Vector2();
    public Vector2 DistanceUnit => _DistanceUnit;
    [SerializeField] [ListDrawerSettings( Expanded = true )] NodeRow[] _Rows;
    public IReadOnlyList<NodeRow> Rows => _Rows;

    public IReadOnlyList<IReadOnlyList<NodeType>> ToNodesList(){
        return Rows.Select(row => row.Nodes.ToList()).ToList();
    }

    [Button][ExecuteInEditMode]
    public void GoToTestSequence(){
        if(!GameData.Instance.TestSequnce.Stages.Contains(this)) GameData.Instance.TestSequnce.AddToFirst(this);
        else GameData.Instance.TestSequnce.MoveToFirst(this);
    }
    [Button][ExecuteInEditMode]
    public void CopyArray(StageData src){
        _Rows = new NodeRow[src.Rows.Count];
        for(int i = 0; i < src.Rows.Count; i++){
            _Rows[i] = new NodeRow();
            _Rows[i].CopyFrom(src.Rows[i]);
        }
    }
    [Button][ExecuteInEditMode]
    public void CreateGrid(int rows, int columns, NodeType type){
        _Rows = new NodeRow[rows];
        for(int i = 0; i < rows; i++){
            _Rows[i] = new NodeRow(
                Enumerable.Repeat<NodeType>(type, columns).ToArray()
            );
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

    [ExecuteInEditMode]
    public NodeRow(NodeType[] arr){
        _Nodes = arr;
    }
    public NodeRow(){}

    [Button] [ExecuteInEditMode]
    public void Fill(NodeType type){
        for(int i = 0; i < Nodes.Count; i++){
            _Nodes[i] = type;
        }
    }
}
