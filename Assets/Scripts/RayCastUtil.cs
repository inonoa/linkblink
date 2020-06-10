using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RayCastUtil
{
    public static RayHitInfo RayHit(Vector3 origin, Vector3 target2Dir){

        var hits = Physics.RaycastAll(origin, (target2Dir - origin).normalized , Mathf.Infinity);
        if(hits.Count() > 0){
            var sorted = hits
                         .Where(hit => hit.collider.GetComponentInParent<ITouchableByMouse>() != null)
                         .OrderBy(hit => hit.distance)
                         .Select<RaycastHit, RayHitInfo>(hit => {
                             var touchable = hit.collider.GetComponentInParent<ITouchableByMouse>();
                             return new RayHitInfo(touchable, hit.point);
                         });
            return sorted.ElementAtOrDefault(0);
        }else{
            return null;
        }
    }

    public static IEnumerable<RayHitInfo> RayHitsFromCamera(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.RaycastAll(ray.origin, ray.direction, Mathf.Infinity)
               .Where(hit => hit.collider.GetComponentInParent<ITouchableByMouse>() != null)
               .OrderBy(hit => hit.distance)
               .Select<RaycastHit, RayHitInfo>(hit => {
                   var touchable = hit.collider.GetComponentInParent<ITouchableByMouse>();
                   return new RayHitInfo(touchable, hit.point);
               });
    }
}

public class RayHitInfo{
    public RayHitInfo(ITouchableByMouse touchable, Vector3 hitPos){
        (this.Hit, this.hitPos) = (touchable, hitPos);
    }
    public ITouchableByMouse Hit{ get; private set; }
    public Vector3 hitPos{ get; private set; }
}
