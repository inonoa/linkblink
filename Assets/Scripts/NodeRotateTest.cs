using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeRotateTest : MonoBehaviour
{
    Vector3 rs;

    // Start is called before the first frame update
    void Start()
    {
        var r = new Vector3();
        r.x = Random.Range(0, 360);
        r.y = Random.Range(0, 360);
        r.z = Random.Range(0, 360);
        transform.Rotate(r);

        rs = new Vector3();
        rs.x = Random.Range(0, 360);
        rs.y = Random.Range(0, 360);
        rs.z = Random.Range(0, 360);
        rs = rs.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rs);
    }
}
