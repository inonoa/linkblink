using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NodeSoundGroup", menuName = "ScriptableObjects/Node Sound Group", order = 2)]
public class NodeSoundGroup : ScriptableObject
{
    [SerializeField] SoundAndVolume _OnMouseOnSound;
    public SoundAndVolume OnMouseOnSound => _OnMouseOnSound;
    [SerializeField] SoundAndVolume[] _OnSelectedSounds;
    public IReadOnlyList<SoundAndVolume> OnSelectedSounds => _OnSelectedSounds;
    [SerializeField] SoundAndVolume _OnVanishLastSound;
    public SoundAndVolume OnVanishLastSound => _OnVanishLastSound;
    [SerializeField] SoundAndVolume _OnVanishSound;
    public SoundAndVolume OnVanishSound => _OnVanishSound;
    [SerializeField] SoundAndVolume _OnAwakeSound;
    public SoundAndVolume OnAwakeSound => _OnAwakeSound;
}
