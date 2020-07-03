using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Link
{

    List<NodeMover> _Nodes = new List<NodeMover>();
    public IReadOnlyList<NodeMover> Nodes => _Nodes;
    public int Count => _Nodes.Count;

    List<Beam> _Beams = new List<Beam>();
    public IReadOnlyList<Beam> Beams => _Beams;
    public Beam CurrentBeam => _Beams.Last();

    HashSet<NodeColor> _Colors = new HashSet<NodeColor>();
    public IReadOnlyCollection<NodeColor> Colors => _Colors;
    public bool HasSameColor(IEnumerable<NodeColor> colors){
        return _Colors.Intersect(colors).Count() > 0;
    }


    public void Add(NodeMover node, Beam beam){
        _Nodes.Add(node);
        _Beams.Add(beam);

        bool hadMultipleColors = (_Colors.Count > 1) || (node.Colors.Length > 1);
        if(_Nodes.Count == 1) _Colors = new HashSet<NodeColor>(node.Colors);
        else _Colors.IntersectWith(node.Colors);
        if(hadMultipleColors && _Colors.Count == 1){
            _Beams.ForEach(bm => bm.SetColor(_Colors.ElementAt(0).ToType()));
        }
    }

    public void OnClear(){
        _Beams.ForEach(bm => bm.Vanish());
    }
}
