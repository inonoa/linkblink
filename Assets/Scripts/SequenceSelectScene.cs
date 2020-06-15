using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SequenceSelectScene : MonoBehaviour
{
    [SerializeField] GridLayouter sequenceNodesParent;
    [SerializeField] NodeMover nodePrefab;

    public void Init(IReadOnlyList<Sequence> sequences){

        gameObject.SetActive(true);
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, 0.5f);

        for(int i = 0; i < 100; i++){
            NodeMover node = Instantiate(nodePrefab);
            sequenceNodesParent.Children.Add(node.transform);
            //node.Clicked += (_, __) => OnSequenceSelected(seq);
        }
    }

    void OnSequenceSelected(Sequence sequence){
        print(sequence.Data.Name);
    }
}
