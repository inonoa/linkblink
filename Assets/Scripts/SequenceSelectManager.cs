using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class SequenceSelectManager : MonoBehaviour
{
    [SerializeField] NodeMover normalPrefab;
    [SerializeField] NodeMover hardPrefab;
    NodeMover normalNode;
    NodeMover hardNode;
    [SerializeField][ReadOnly] Sequence normal;
    [SerializeField][ReadOnly] Sequence hard;

    [SerializeField] StageWatcher stageWatcher;
    [SerializeField] StageUIsHolder stageUIsHolder;

    public void InitSequences(Sequence normal, Sequence hard){
        (this.normal, this.hard) = (normal, hard);
    }

    public void Init(){
        normalNode = GameObject.Instantiate(normalPrefab);
        hardNode = GameObject.Instantiate(hardPrefab);

        normalNode.Clicked += OnNormalSequenceSelected;
        hardNode.Clicked += OnHardSequenceSelected;

        gameObject.SetActive(true);
        GetComponent<CanvasGroup>().DOFade(1, 0.5f);
    }

    void OnNormalSequenceSelected(object _, EventArgs __){
        OnSequenceSelected(normal);
    }

    void OnHardSequenceSelected(object _, EventArgs __){
        OnSequenceSelected(hard);
    }

    void OnSequenceSelected(Sequence seq){
        hardNode.Vanish();
        normalNode.Vanish();
        hardNode.Clicked -= OnHardSequenceSelected;
        normalNode.Clicked -= OnNormalSequenceSelected;

        GetComponent<CanvasGroup>().DOFade(0, 0.5f);
        DOVirtual.DelayedCall(0.5f, () => gameObject.SetActive(false));
        
        DOVirtual.DelayedCall(0.5f, () => {
            stageUIsHolder.ForEach(group => group.DOFade(1, 0.5f));
        });

        stageWatcher.InitSequence(seq);
    }


    void Update()
    {
        
    }
}
