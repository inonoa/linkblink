using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class StageUIsHolder : MonoBehaviour
{
    [SerializeField] CanvasGroup[] _Groups;
    public IReadOnlyList<CanvasGroup> Groups => _Groups;

    public void ForEach(Action<CanvasGroup> action){
        foreach(CanvasGroup group in Groups){
            action.Invoke(group);
        }
    }
}
