﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SequenceSelectScene : MonoBehaviour
{
    [SerializeField] GridLayouter sequenceNodesParent;
    [SerializeField] NodeMover nodePrefab;
    [SerializeField] StageWatcher stageWatcher;
    [SerializeField] SequenceEnterManager sequenceEnterManager;
    [SerializeField] TextTextureRenderer tTRenderer;

    List<NodeMover> nodes = new List<NodeMover>();

    IReadOnlyList<Sequence> sequences;


    List<Tween> tweens = new List<Tween>();

    public void Init(IReadOnlyList<Sequence> sequences){
        this.sequences = sequences;

        ReStart();
    }

    public void ReStart(){
        tweens.ForEach(tw => tw.Kill());
        tweens.Clear();

        gameObject.SetActive(true);
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        tweens.Add(canvasGroup.DOFade(1, 0.5f));
        
        sequenceNodesParent.Children.Clear();

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

        tweens.Add(GetComponent<CanvasGroup>().DOFade(0, 0.5f));
        tweens.Add(DOVirtual.DelayedCall(1f, () => gameObject.SetActive(false)));

        //stageWatcher.Init(sequence);
        sequenceEnterManager.Init(sequence);
    }
}
