﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DebugParameters", menuName = "ScriptableObjects/DebugParameters", order = 3)]
public class DebugParameters : ScriptableObject
{
    [SerializeField] int _StartStage;
    public int StartStage => _StartStage;

    [SerializeField] bool _LoopSameStage;
    public bool LoopSameStage => _LoopSameStage;
    
    [SerializeField] LinkTriggerType _LinkTrigger = LinkTriggerType.Click;
    public LinkTriggerType LinkTrigger => _LinkTrigger;


    #region Singleton じゃなくね
    public DebugParameters(){
        Debug.Assert(Instance == null);
        Instance = this;
    }
    static public DebugParameters Instance{ get; private set; }
    #endregion
}

public enum LinkTriggerType{
    Click, MouseOver
}
