using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable]
public class NodeTypeData
{
    [SerializeField] NodeType _Type;
    public NodeType Type => _Type;

    [field: SerializeField] [field: LabelText("Prefab")]
    public NodeMover Prefab{ get; private set; }
    

    [field: SerializeField] [field: LabelText("EditorColor")]
    public Color EditorColor{ get; private set; } = new Color(0,0,0,1);

    [field: SerializeField] [field: LabelText("BeamColor")]
    public Color BeamColor{ get; private set; } = new Color(0,0,0,1);
    
    [field: SerializeField] [field: LabelText("BeamColor2")]
    public Color BeamColor2{ get; private set; } = new Color(0,0,0,1);

    [field: SerializeField] [field: LabelText("BeamEmit")]
    public Color BeamEmit{ get; private set; } = new Color(0,0,0,1);

    [field: SerializeField] [field: LabelText("PointTextColor")]
    public Color PointTextColor{ get; private set; } = new Color(0,0,0,1);
    
    
    

}
