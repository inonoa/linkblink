using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCounter
{
    static bool inStartStage = true;
    static int _StageNow = 0;
    public static int StageNow{
        get{
            if(inStartStage){
                _StageNow = DebugParameters.Instance.StartStage;
                inStartStage = false;
            }
            return _StageNow;
        }
        private set{
            _StageNow = value;
        }
    }

    public static void NextStage(){
        if(!DebugParameters.Instance.LoopSameStage) StageNow ++;
    }

    public static void Reset(){
        inStartStage = true;
    }
}
