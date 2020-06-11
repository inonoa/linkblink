using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shutter : TouchableByMouse
{
    public override bool ShutOutRay => true;
}
