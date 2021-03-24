using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCommon : MonoBehaviour
{
    public void PlaySound(string SoundName)
    {
        MusicAndSoundManager.PlaySound(SoundName, Camera.main.transform.position);
    }
}
