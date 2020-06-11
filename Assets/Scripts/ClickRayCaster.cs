using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ClickRayCaster : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;

    public bool MouseOn(TouchableByMouse touchable, out Vector3 hitPos){
        bool contains = hitByRay.ContainsKey(touchable);
        hitPos = contains ? hitByRay[touchable] : new Vector3();
        return contains;
    }

    public bool MouseOn(TouchableByMouse touchable){
        return hitByRay.ContainsKey(touchable);
    }

    Dictionary<TouchableByMouse, Vector3> hitByRay = new Dictionary<TouchableByMouse, Vector3>();
    public RayHitInfo HitFirst{ get; private set; }
    void Update()
    {
        hitByRay.Clear();

        var hits = RayCastUtil.RayHitsFromCamera(layerMask);
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
