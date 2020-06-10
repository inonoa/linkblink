using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] Text text;

    int _Score;
    public int Score => _Score;

    public void ResetCurrentScore(){
        _Score = 0;
        text.text = 0.ToString();
    }

    public void LinkToScore(Vector3[] nodePositions, Vector2 distanceUnit){

        //print(distanceUnit);
        print("=====");

        //一応整数に揃えておく（必要か？？）、単位で割る
        float[,] posGrided = new float[nodePositions.Length, 2];
        {
            Vector2 firstPos = new Vector2(
                nodePositions[0].x / distanceUnit.y,
                nodePositions[0].y / distanceUnit.y
            );
            print(firstPos);
            for(int i = 0; i < nodePositions.Length; i++){
                posGrided[i, 0] = nodePositions[i].x / distanceUnit.y - firstPos.x;
                posGrided[i, 1] = nodePositions[i].y / distanceUnit.y - firstPos.y;

                print(posGrided[i, 0] + "," + posGrided[i, 1]);
            }
        }

        int distanceScore = 0;
        {
            const int distanceRate = 40;
            int len = posGrided.GetLength(0);
            for(int i=0;i<posGrided.GetLength(0);i++){
                //print(posGrided[i, 0] + "," + posGrided[i, 1]);
            }
            for(int i = 0; i < len; i++){
                (float dx, float dy) = (
                    posGrided[i, 0] - posGrided[(i+1) % len, 0],
                    posGrided[i, 1] - posGrided[(i+1) % len, 1]
                );
                float thisdist = Mathf.Sqrt(Mathf.Pow(dx, 2) + Mathf.Pow(dy, 2)) * distanceRate;
                int thisScore = Mathf.RoundToInt(thisdist / 10) * 10; //10点刻みにしたい
                distanceScore += thisScore;
            }

        }

        int numScore = 0;
        {
            int[] num2Score = {0, 0, 0, 100, 200, 300, 500, 700, 1000, 1400, 1900, 2500, 3200, 4000};
            numScore = num2Score[nodePositions.Length];

        }

        int score = distanceScore + numScore;

        _Score += score;

        text.text = _Score.ToString();
    }
}
