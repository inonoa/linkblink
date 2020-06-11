using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TouchableByMouse : MonoBehaviour
{
    public abstract bool ShutOutRay{ get; }
}
