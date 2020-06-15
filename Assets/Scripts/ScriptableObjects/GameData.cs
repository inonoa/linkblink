using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Game", menuName = "ScriptableObjects/GameData", order = 3)]
public class GameData : ScriptableObject{
    [SerializeField] SequenceData[] _Sequences;
    public IReadOnlyList<SequenceData> Sequences => _Sequences;

    public SequenceData Normal => Sequences[0];
    public SequenceData Hard => Sequences[1];

    #region Singletonではないが
    public GameData(){
        Debug.Assert(Instance == null);
        Instance = this;
    }
    static public GameData Instance{ get; private set; }

    #endregion

    [ExecuteInEditMode] public void Add(SequenceData sequence) => _Sequences = _Sequences.Append(sequence).ToArray();
}
