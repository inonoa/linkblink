using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] StageUIsHolder stageUIsHolder;
    [SerializeField] SequenceSelectManager sequenceSelectManager;
    [SerializeField] Transform titleBG;
    Game game;

    void Start()
    {
        startButton.onClick.AddListener(OnStartButtonClick);
        stageUIsHolder.ForEach(group => {
            group.alpha = 0;
            group.gameObject.SetActive(false);
        });

        game = new Game(GameData.Instance);
        sequenceSelectManager.InitSequences(game.Sequences[0], game.Sequences[1]);
    }

    public void Init(){
        startButton.interactable = true;
        gameObject.SetActive(true);
        GetComponent<CanvasGroup>().DOFade(1, 0.5f);
    }


    void Update()
    {
        
    }

    void OnStartButtonClick(){
        startButton.interactable = false;
        DOVirtual.DelayedCall(0.2f, () => GetComponent<CanvasGroup>().DOFade(0, 0.5f));
        DOVirtual.DelayedCall(0.7f, () => gameObject.SetActive(false));

        foreach(IVanish iVanish in titleBG.GetComponentsInChildren<IVanish>()){
            iVanish.Vanish();
        }

        DOVirtual.DelayedCall(0.5f, () => {
            sequenceSelectManager.Init();
        });
    }
}
