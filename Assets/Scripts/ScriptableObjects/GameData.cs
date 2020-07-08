using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Game", menuName = "ScriptableObjects/GameData", order = 3)]
public class GameData : ScriptableObject{
    [SerializeField] SequenceData[] _Sequences;
    public IReadOnlyList<SequenceData> Sequences => _Sequences;

    public SequenceData TestSequnce => Sequences.First(seq => seq.Name == "Test");

    #region Singletonではないが
    public GameData(){
        Debug.Assert(Instance == null);
        Instance = this;
    }
    static public GameData Instance{ get; private set; }

    #endregion

    [ExecuteInEditMode]
    public void Add(SequenceData sequence){
        _Sequences = _Sequences.Append(sequence).ToArray();
        EditorUtility.SetDirty(this);
    }


    [ExecuteInEditMode][Button]
    public void OverlapTest(){
        Dictionary<StageData, (string seqName, int index)> datas = new Dictionary<StageData, (string seqName, int index)>();
        for(int i = 0; i < Sequences.Count; i++){
            for(int j = 0; j < Sequences[i].Stages.Count; j++){
                if(Sequences[i].Stages[j] == null) continue;
                if(datas.ContainsKey(Sequences[i].Stages[j])){
                    Debug.LogError($"{datas[Sequences[i].Stages[j]].seqName}の{datas[Sequences[i].Stages[j]].index}番目と"
                                  + $"{Sequences[i].Name}の{j+1}番目が重複しています");
                }else{
                    datas.Add(Sequences[i].Stages[j], (Sequences[i].Name, j+1));
                }
            }
        }
        "おわりー".print();
    }
}
