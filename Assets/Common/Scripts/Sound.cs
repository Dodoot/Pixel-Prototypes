using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sound")]
public class Sound : ScriptableObject
{
    [SerializeField] string soundName = null;
    [SerializeField] AudioClip soundClip = null;
    [SerializeField] float soundVolume = 0.5f;

    public string GetSoundName() { return soundName; }
    public AudioClip GetSoundClip() { return soundClip; }
    public float GetSoundVolume() { return soundVolume; }
}