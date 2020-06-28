using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DialogManager : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;

    public event EventHandler dialogClosed;

    void Start()
    {

    }

    void Update(){
        //if(Input.GetKeyDown(KeyCode.X)) Init();
    }

    public void Init(){
        GetComponent<Canvas>().worldCamera = Camera.main;
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, 0.5f);
        canvasGroup.transform.localPosition -= new Vector3(0, 50, 0);
        canvasGroup.transform.DOLocalMoveY(50, 0.5f).SetRelative().SetEase(Ease.OutQuint);
    }


    public void OnButtonClick()
    {
        canvasGroup.DOFade(0, 0.5f);
        canvasGroup.transform.DOLocalMoveY(50, 0.5f).SetRelative().SetEase(Ease.InSine);
        DOVirtual.DelayedCall(0.5f, () => {
            Destroy(gameObject);
            dialogClosed?.Invoke(this, EventArgs.Empty);
        });
    }
}
