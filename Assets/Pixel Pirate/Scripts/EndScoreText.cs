using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScoreText : MonoBehaviour
{
    [SerializeField] Text scoreText = null;

    void Start()
    {
         scoreText.text = FindObjectOfType<MemoryBetweenScenes>().GetScore().ToString();
    }
}
