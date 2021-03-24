using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    // Fixed game settings
    [SerializeField] int myTargetFramerate = 60;

    void Start()
    {
        Application.targetFrameRate = myTargetFramerate;
    }

    void Update()
    {
        
    }
}
