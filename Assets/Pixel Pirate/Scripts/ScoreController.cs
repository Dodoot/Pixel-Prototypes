using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Text scoreText = null;

    int currentScore;

    private void Start()
    {
        currentScore = 0;
        scoreText.text = currentScore.ToString();
    }

    public void AddToScore(int scoreToAdd)
    {
        currentScore += scoreToAdd;
        scoreText.text = currentScore.ToString();
    }

    public void SendScoreToMemory()
    {
        FindObjectOfType<MemoryBetweenScenes>().SetScore(currentScore);
    }
}
