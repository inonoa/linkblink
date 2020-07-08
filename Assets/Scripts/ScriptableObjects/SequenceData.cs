using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;

[CreateAssetMenu(fileName = "Sequence", menuName = "ScriptableObjects/SequenceData", order = 2)]
public class SequenceData : ScriptableObject
{
    public string Name{
        get{
            Debug.Assert(!string.IsNullOrEmpty(_Name));
            return _Name;
        }
    }
    [SerializeField] string _Name;
    [SerializeField, TextArea] string memo;
    public IReadOnlyList<StageData> Stages => _Stages;
    [SerializeField] StageData[] _Stages;
    

    public void AddToFirst(StageData stage){
        _Stages = new StageData[]{ stage }.Concat(_Stages).ToArray();
        EditorUtility.SetDirty(this);
    }
    public void MoveToFirst(StageData stage){
        _Stages = _Stages.ToList()
                         .Except(new StageData[]{ stage })
                         .Prepend(stage)
                         .ToArray();
        EditorUtility.SetDirty(this);
    }

    [Button]
    void AddToGameData(){
        GameData.Instance.Add(this);
    }
    [ExecuteInEditMode]
    public void Add(StageData stage){
        _Stages = _Stages.Append(stage).ToArray();
        EditorUtility.SetDirty(this);
    }
}
