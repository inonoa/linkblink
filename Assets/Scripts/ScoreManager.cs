using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using System;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] int[] pointsPerNode;
    [SerializeField] int beamPointRate = 20;

    int _Score;
    public int Score => _Score;

    public void ResetCurrentScore(){
        _Score = 0;
        text.text = 0.ToString();
    }

    public void AddScore(int delta){
        _Score += delta;
        text.text = _Score.ToString();
    }

    public PointInfo LinkToScore(Vector3[] nodePositions){

        PointInfo point = new PointInfo(nodePositions.Length);

        nodePositions.ForEach((i, pos) => 
            point.nodePoints[i] = new Tuple<Vector3, int>(pos, pointsPerNode[nodePositions.Length])
        );

        for(int i = 0; i < nodePositions.Length - 1; i++){
            
            point.beamPoints[i] = new Tuple<Vector3, int>(
                (nodePositions[i] + nodePositions[i+1]) / 2,
                (Mathf.RoundToInt(beamPointRate * Vector3.Distance(nodePositions[i], nodePositions[i+1])) + 5) / 10 * 10
            );
        }
        point.beamPoints[nodePositions.Length - 1] = new Tuple<Vector3, int>(
            (nodePositions[nodePositions.Length - 1] + nodePositions[0]) / 2,
            (Mathf.RoundToInt(beamPointRate * Vector3.Distance(nodePositions[nodePositions.Length - 1], nodePositions[0])) + 5) / 10 * 10
        );

        return point;
    }
}
