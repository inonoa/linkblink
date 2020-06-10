using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SoundAndVolume
{
    [SerializeField] AudioClip clip;
    [SerializeField, Range(0, 1)] float volume = 1;
    public void Play(float volumeRate = 1){
        SoundUtil.PlayOneShot(clip, volume * volumeRate);
    }
}
