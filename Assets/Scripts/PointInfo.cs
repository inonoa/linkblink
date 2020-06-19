using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PointInfo : MonoBehaviour
{
    public Tuple<Vector3, int>[] nodePoints;
    public Tuple<Vector3, int>[] beamPoints;
    public int TotalPoint() => nodePoints.Sum(np => np.Item2) + beamPoints.Sum(bp => bp.Item2);

    public PointInfo(int numNodes){
        nodePoints = new Tuple<Vector3, int>[numNodes];
        beamPoints = new Tuple<Vector3, int>[numNodes];
    }
}
