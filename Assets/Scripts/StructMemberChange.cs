using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StructMemberChange
{
    public static Vector3 Xto(this Vector3 vec, float x){
        vec.x = x; return vec;
    }
    public static Vector3 Yto(this Vector3 vec, float y){
        vec.y = y; return vec;
    }
    public static Vector3 Zto(this Vector3 vec, float z){
        vec.z = z; return vec;
    }

    public static Vector3 Xplus(this Vector3 vec, float delta){
        vec.x += delta; return vec;
    }
    public static Vector3 Yplus(this Vector3 vec, float delta){
        vec.y += delta; return vec;
    }
    public static Vector3 Zplus(this Vector3 vec, float delta){
        vec.z += delta; return vec;
    }

    public static Vector3 Xminus(this Vector3 vec, float delta){
        vec.x -= delta; return vec;
    }
    public static Vector3 Yminus(this Vector3 vec, float delta){
        vec.y -= delta; return vec;
    }
    public static Vector3 Zminus(this Vector3 vec, float delta){
        vec.z -= delta; return vec;
    }

    public static Color Rto(this Color color, float r){
        color.r = r; return color;
    }
    public static Color Gto(this Color color, float g){
        color.g = g; return color;
    }
    public static Color Bto(this Color color, float b){
        color.b = b; return color;
    }
    public static Color Ato(this Color color, float a){
        color.a = a; return color;
    }

    public static Color Rplus(this Color color, float delta){
        color.r += delta; return color;
    }
    public static Color Gplus(this Color color, float delta){
        color.g += delta; return color;
    }
    public static Color Bplus(this Color color, float delta){
        color.b += delta; return color;
    }
    public static Color Aplus(this Color color, float delta){
        color.a += delta; return color;
    }

    public static Color Rminus(this Color color, float delta){
        color.r -= delta; return color;
    }
    public static Color Gminus(this Color color, float delta){
        color.g -= delta; return color;
    }
    public static Color Bminus(this Color color, float delta){
        color.b -= delta; return color;
    }
    public static Color Aminus(this Color color, float delta){
        color.a -= delta; return color;
    }
}
