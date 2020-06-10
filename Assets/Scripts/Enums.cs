using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType{
    None, Cyan, Magenta, Yellow, Green, CyanMagenta, Black, AllColor
}

public enum NodeColor{
    Cyan, Magenta, Yellow, Green
}

public static class Color2TypeExtension{

    public static NodeType ToType(this NodeColor color){
        switch(color){
            case NodeColor.Cyan:    return NodeType.Cyan;
            case NodeColor.Magenta: return NodeType.Magenta;
            case NodeColor.Yellow:  return NodeType.Yellow;
            case NodeColor.Green:   return NodeType.Green;
            default: return NodeType.None;
        }
    }
}
