using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MaterialExtension
{

    public static void AddFloat(this Material material, string name, float amount){
        material.SetFloat(name, material.GetFloat(name) + amount);
    }
    public static void AddColor(this Material material, string name, Color amount){
        material.SetColor(name, material.GetColor(name) + amount);
    }
}
