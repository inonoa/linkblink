using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundUtil : MonoBehaviour
{
    static AudioSource player;

    void Start(){
        player = GetComponent<AudioSource>();
    }


    public static void PlayOneShot(AudioClip clip, float volume = 1){
        Debug.Assert(player != null);

        player.PlayOneShot(clip, volume);
    }
}
