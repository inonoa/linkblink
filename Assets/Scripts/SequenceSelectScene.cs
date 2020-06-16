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
    [SerializeField] StageUIsHolder stageUIsHolder;
    [SerializeField] StageWatcher stageWatcher;
    [SerializeField] TextTextureRenderer tTRenderer;

    List<NodeMover> nodes = new List<NodeMover>();

    public void Init(IReadOnlyList<Sequence> sequences){

        gameObject.SetActive(true);
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, 0.5f);

        foreach(Sequence seq in sequences){
            NodeMover node = Instantiate(nodePrefab);
            sequenceNodesParent.Children.Add(node.transform);
            node.Clicked += (_, __) => OnSequenceSelected(seq);
            node.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", tTRenderer.Render(seq.Data.Name));
            nodes.Add(node);
        }
    }

    void OnSequenceSelected(Sequence sequence){
        nodes.ForEach(node => node.Vanish());
        nodes.Clear();

        GetComponent<CanvasGroup>().DOFade(0, 0.5f);
        DOVirtual.DelayedCall(1f, () => gameObject.SetActive(false));
        
        DOVirtual.DelayedCall(0.5f, () => {
            stageUIsHolder.ForEach(group => {
                group.gameObject.SetActive(true);
                group.DOFade(1, 0.5f);
            });
        });

        stageWatcher.Init(sequence);
    }
}
