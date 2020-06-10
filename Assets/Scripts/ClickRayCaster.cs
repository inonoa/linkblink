using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ClickRayCaster : MonoBehaviour
{
    public bool MouseOn(ITouchableByMouse touchable, out Vector3 hitPos){
        bool contains = hitByRay.ContainsKey(touchable);
        hitPos = contains ? hitByRay[touchable] : new Vector3();
        return contains;
    }

    public bool MouseOn(ITouchableByMouse touchable){
        return hitByRay.ContainsKey(touchable);
    }

    Dictionary<ITouchableByMouse, Vector3> hitByRay = new Dictionary<ITouchableByMouse, Vector3>();
    public RayHitInfo HitFirst{ get; private set; }
    void Update()
    {
        hitByRay.Clear();

        var hits = RayCastUtil.RayHitsFromCamera();
        bool first = true;
        foreach(RayHitInfo hit in hits){
            hitByRay.Add(hit.Hit, hit.hitPos);
            if(first){
                HitFirst = hit;
                first = false;
            }
            if(hit.Hit.ShutOutRay) break;
        }

        if(first) HitFirst = null;
    }


    static public ClickRayCaster Instance{ get; private set; }
    void Awake() => Instance = this;

    
}
