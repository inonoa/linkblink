using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RayCastUtil
{
    public static RayHitInfo RayHit(Vector3 origin, Vector3 target2Dir, int layerMask){

        var hits = Physics.RaycastAll(origin, (target2Dir - origin).normalized , Mathf.Infinity, layerMask);
        if(hits.Count() > 0){
            var sorted = hits
                         .Where(hit => hit.collider.GetComponentInParent<TouchableByMouse>() != null)
                         .OrderBy(hit => hit.distance)
                         .Select<RaycastHit, RayHitInfo>(hit => {
                             var touchable = hit.collider.GetComponentInParent<TouchableByMouse>();
                             return new RayHitInfo(touchable, hit.point);
                         });
            return sorted.ElementAtOrDefault(0);
        }else{
            return null;
        }
    }

    public static IEnumerable<RayHitInfo> RayHitsFromCamera(int layerMask){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.RaycastAll(ray.origin, ray.direction, Mathf.Infinity, layerMask)
               .Where(hit => hit.collider.GetComponentInParent<TouchableByMouse>() != null)
               .OrderBy(hit => hit.distance)
               .Select<RaycastHit, RayHitInfo>(hit => {
                   var touchable = hit.collider.GetComponentInParent<TouchableByMouse>();
                   return new RayHitInfo(touchable, hit.point);
               });
    }
}

public class RayHitInfo{
    public RayHitInfo(TouchableByMouse touchable, Vector3 hitPos){
        (this.Hit, this.hitPos) = (touchable, hitPos);
    }
    public TouchableByMouse Hit{ get; private set; }
    public Vector3 hitPos{ get; private set; }
}
