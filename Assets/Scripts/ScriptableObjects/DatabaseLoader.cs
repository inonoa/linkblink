using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseLoader : MonoBehaviour
{
    [SerializeField] ScriptableObject[] datasToLoad;

    [RuntimeInitializeOnLoadMethod]
    static void Load(){
        Instantiate(Resources.Load<DatabaseLoader>("DatabaseLoader"));
    }
}
