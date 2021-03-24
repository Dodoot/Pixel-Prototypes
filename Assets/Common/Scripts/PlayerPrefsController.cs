using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsController : MonoBehaviour
{
    const string SOUND_ON_KEY = "sound on";
    // const string DALTON_MODE_KEY = "dalton on";

    public static int GetSoundOn() { return PlayerPrefs.GetInt(SOUND_ON_KEY, 1); }
    // public static int GetDaltonMode() { return PlayerPrefs.GetInt(DALTON_MODE_KEY, 0); }

    public static void ResetAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        // todo reset all stuff in memory too
    }

    public static void SwitchSoundOn()
    {
        if (GetSoundOn() == 0)
        {
            PlayerPrefs.SetInt(SOUND_ON_KEY, 1);
        }
        else
        {
            PlayerPrefs.SetInt(SOUND_ON_KEY, 0);
        }
    }

    public static void SetSoundOn(int newSoundOn)
    {
        PlayerPrefs.SetInt(SOUND_ON_KEY, newSoundOn);
    }

    // public static void SetDaltonMode(int newDaltonMode)
    // {
    //     PlayerPrefs.SetInt(DALTON_MODE_KEY, newDaltonMode);
    // }
}