using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] float rotateSpeedPerSec = 50;

    IEnumerator coroutine;
    void Start()
    {
        (float rotateX, float rotateY, float rotateZ)
            = (Rand0_360(), Rand0_360(), Rand0_360());
        transform.Rotate(rotateX, rotateY, rotateZ);

        (rotateX, rotateY, rotateZ)
            = (Rand0_360(), Rand0_360(), Rand0_360());
        coroutine = Rotate(new Vector3(rotateX, rotateY, rotateZ).normalized);
        StartCoroutine(coroutine);
    }
    float Rand0_360(){
        return UnityEngine.Random.Range(0, 360);
    }

    public void Stop(){
        if(coroutine != null) StopCoroutine(coroutine);
    }

    IEnumerator Rotate(Vector3 rotationNormalized){
        while(true){
            yield return null;

            Vector3 rot = rotationNormalized * Time.deltaTime * rotateSpeedPerSec;
            transform.Rotate(rot.x, rot.y, rot.z);
        }
    }
}
