using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StageNumColumns : MonoBehaviour
{
    [SerializeField] Texture2D[] texs;
    [SerializeField] MeshRenderer[] columns;
    [SerializeField] float scrollSpeed = 0.1f;

    [SerializeField] StageWatcher stageWatcher;

    void Start()
    {
        StartCoroutine(Scroll());
    }


    IEnumerator Scroll(){
        
        int idx = (stageWatcher.PlayMode == StageWatcher.EPlayMode.Sequence) ? stageWatcher.CurrentStageIndex : 0;
        foreach(MeshRenderer clm in columns){
            clm.material.SetTexture("_MainTex", texs[ stageWatcher.CurrentStageIndex]);
        }

        Vector4[] tiling_offsets = columns.Select(clm => clm.material.GetVector("_MainTex_ST")).ToArray();

        while(true){
            for(int i = 0; i < columns.Length; i++){
                tiling_offsets[i].w -= Time.deltaTime * scrollSpeed;
                while(tiling_offsets[i].w < 0){
                    tiling_offsets[i].w += 1;
                    int idx_now = (stageWatcher.PlayMode == StageWatcher.EPlayMode.Sequence) ? stageWatcher.CurrentStageIndex : 0;
                    columns[i].material.SetTexture("_MainTex", texs[idx_now]);
                }
                columns[i].material.SetVector("_MainTex_ST", tiling_offsets[i]);
            }

            yield return null;
        }
    }
}
