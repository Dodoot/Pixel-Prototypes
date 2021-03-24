using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryBetweenScenes : MonoBehaviour
{
    int score = -1;
    int lastSceneIndex;

    // Singleton pattern
    private void Awake()
    {
        int numMemories = FindObjectsOfType<MemoryBetweenScenes>().Length;
        if (numMemories > 1) { Destroy(gameObject); }
        else { DontDestroyOnLoad(gameObject); }
    }

    public int GetScore() { return score; }
    public void SetScore(int newScore) { score = newScore; }
    public int GetLastSceneIndex() { return lastSceneIndex; }
    public void SetLastSceneIndex(int newLastSceneIndex) { lastSceneIndex = newLastSceneIndex; }
}
