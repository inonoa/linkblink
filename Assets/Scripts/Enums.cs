using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType{
    None, Cyan, Magenta, Yellow, Green, CyanMagenta, Black, AllColor, CyanYellow, CyanGreen, MagentaYellow, MagentaGreen, YellowGreen
}

public enum NodeColor{
    Cyan, Magenta, Yellow, Green
}

public static class ColorTypeExtension{

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
            case NodeType.CyanYellow:  return new NodeColor[]{ NodeColor.Cyan, NodeColor.Yellow };
            case NodeType.CyanGreen:   return new NodeColor[]{ NodeColor.Cyan, NodeColor.Green };
            case NodeType.MagentaYellow: return new NodeColor[]{ NodeColor.Magenta, NodeColor.Yellow };
            case NodeType.MagentaGreen:  return new NodeColor[]{ NodeColor.Magenta, NodeColor.Green };
            case NodeType.YellowGreen: return new NodeColor[]{ NodeColor.Yellow, NodeColor.Green };

            case NodeType.Black: return new NodeColor[]{};
            case NodeType.AllColor: return Enum.GetValues(typeof(NodeColor)) as NodeColor[];

            default: return new NodeColor[]{};
        }
    }

    public static bool HasTwoColors(this NodeType type){
        return    type == NodeType.CyanMagenta
               || type == NodeType.CyanYellow
               || type == NodeType.CyanGreen
               || type == NodeType.MagentaYellow
               || type == NodeType.MagentaGreen
               || type == NodeType.YellowGreen;
    }
}
