using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageLoder : MonoBehaviour
{
    [SerializeField] BoardManager boardPrefab;
    [SerializeField] GameObject rowPrefab;

    public BoardManager LoadBoard(StageData stage){

        BoardManager board = Instantiate(boardPrefab);
        if(stage.DistanceUnit == Vector2.zero){
            board.NodeDistanceUnit = new Vector2(
                6.0f / (stage.Rows[0].Nodes.Count - 1),
                6.0f / (stage.Rows.Count - 1)
            );
        }else{
            board.NodeDistanceUnit = stage.DistanceUnit;
        }
        for(int i = 0; i < stage.Rows.Count; i++){
            Transform row = Instantiate(rowPrefab, board.transform).transform;

            
            row.position += new Vector3(0, (stage.Rows.Count / 2f - 0.5f - i) * board.NodeDistanceUnit.y, 0);

            for(int j = 0; j < stage.Rows[i].Nodes.Count; j++){
                
                NodeType type = stage.Rows[i].Nodes[j];
                if(type != NodeType.None){
                    NodeMover node = Instantiate(TypeDataHolder.Instance[type].Prefab, row);
                    node.transform.position += new Vector3((-stage.Rows[i].Nodes.Count / 2f + 0.5f + j) * board.NodeDistanceUnit.x, 0, 0);
                    if(node.Bomb != null) node.Bomb.SetRadius(board.NodeDistanceUnit.y * stage.BombRadius);
                }
            }
        }

        return board;
    }
}
