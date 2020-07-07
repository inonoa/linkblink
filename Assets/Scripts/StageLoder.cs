using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StageLoder : MonoBehaviour
{
    [SerializeField] BoardManager boardPrefab;
    [SerializeField] GameObject rowPrefab;

    [SerializeField] Vector2 stageSizeDefault = new Vector2(10, 6);

    public BoardManager LoadBoard(StageData stage){

        BoardManager board = Instantiate(boardPrefab);

        Vector2Int numNodeGaps = new Vector2Int(
            stage.Rows.Max(row => row.Nodes.Count()) - 1,
            stage.Rows.Count - 1
        );

        //ステージの実際のサイズ決定(todo: 横長だったらxをデフォルトに、そうじゃなかったらyをデフォルトに、とかしたい)
        Vector2 actualSize;
        if(stage.DistanceUnit != Vector2.zero){
            actualSize = numNodeGaps * stage.DistanceUnit;
        }else{
            if((float)numNodeGaps.x / numNodeGaps.y > stageSizeDefault.x / stageSizeDefault.y){
                //横長の場合、横は目一杯広げてyはそれに合わせる
                actualSize.x = stageSizeDefault.x;
                actualSize.y = actualSize.x * ((float) numNodeGaps.y / numNodeGaps.x);
            }else{
                //縦長の場合、縦は目一杯広げてxはそれに合わせる
                actualSize.y = stageSizeDefault.y;
                actualSize.x = actualSize.y * ((float) numNodeGaps.x / numNodeGaps.y);
            }
        }
        Vector2 distanceUnit = actualSize / numNodeGaps;

        for(int i = 0; i < stage.Rows.Count; i++){
            Transform row = Instantiate(rowPrefab, board.transform).transform;

            (float upBound, float downBound) = (actualSize.y / 2, - actualSize.y / 2);
            row.position = new Vector3(0, Mathf.Lerp(upBound, downBound, (float)i / (stage.Rows.Count - 1)), 0);

            for(int j = 0; j < stage.Rows[i].Nodes.Count; j++){
                
                NodeType type = stage.Rows[i].Nodes[j];
                if(type != NodeType.None){
                    NodeMover node = Instantiate(TypeDataHolder.Instance[type].Prefab, row);

                    (float leftBound, float rightBound) = (- actualSize.x / 2, actualSize.x / 2);
                    node.transform.position += new Vector3(
                        Mathf.Lerp(leftBound, rightBound, (float)j / (stage.Rows[i].Nodes.Count - 1)), 0, 0
                    );

                    if(node.Bomb != null) node.Bomb.SetRadius(distanceUnit.y * stage.BombRadius);
                }
            }
        }

        return board;
    }
}
