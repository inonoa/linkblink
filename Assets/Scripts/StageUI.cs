using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageUI : MonoBehaviour
{
    [SerializeField] Text text;
    
    void Start()
    {
        text.text = (StageCounter.StageNow + 1).ToString();
    }

    void Update()
    {
        
    }
}
