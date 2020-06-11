using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType{
    None, Cyan, Magenta, Yellow, Green, CyanMagenta, Black, AllColor,
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

    public static NodeColor[] ToColors(this NodeType type){
        switch(type){
            case NodeType.Cyan:    return new NodeColor[]{ NodeColor.Cyan };
            case NodeType.Magenta: return new NodeColor[]{ NodeColor.Magenta };
            case NodeType.Yellow:  return new NodeColor[]{ NodeColor.Yellow };
            case NodeType.Green:   return new NodeColor[]{ NodeColor.Green };

            case NodeType.CyanMagenta: return new NodeColor[]{ NodeColor.Cyan, NodeColor.Magenta };
            case NodeType.Black: return new NodeColor[]{};
            case NodeType.AllColor: return Enum.GetValues(typeof(NodeColor)) as NodeColor[];

            default: return new NodeColor[]{};
        }
    }
}
