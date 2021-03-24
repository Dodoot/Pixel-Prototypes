using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    [SerializeField] int totalTurns = 3;
    [SerializeField] int playerIndex = 0;
    [SerializeField] GameObject[] racersArray = null;
    [SerializeField] int[] racersProgress;

    int checkpointsPerTurn;

    CR_UIManager myUIManager;
    Animator myAnimator;

    private void Start()
    {
        checkpointsPerTurn = FindObjectOfType<CheckpointManager>().GetNumberOfCheckpoints();

        racersProgress = new int[racersArray.Length];

        myUIManager = FindObjectOfType<CR_UIManager>();
        myAnimator = GetComponent<Animator>();

        StartCountdown();
    }

    public void UpdateProgress(GameObject racer, int numberTurns, int checkpointIndex)
    {
        int racerIndex = Array.IndexOf(racersArray, racer);
        if(racerIndex == -1) { Debug.Log("Racer is not in Race Manager"); return; }

        int newProgress = numberTurns * checkpointsPerTurn + checkpointIndex;

        racersProgress[racerIndex] = newProgress;

        if(racerIndex == playerIndex)
        {
            int racerPosition = GetPosition(racer);

            if (numberTurns == totalTurns)
            {
                FindObjectOfType<MemoryBetweenScenes>().SetScore(racerPosition);
                FindObjectOfType<SceneLoader>().LoadCRMenuEnd();
            }

            myUIManager.UpdateTurnText(numberTurns + 1, totalTurns);
            myUIManager.UpdatePositionText(racerPosition);
        }
    }

    public int GetPosition(GameObject racer)
    {
        int racerIndex = Array.IndexOf(racersArray, racer);
        if (racerIndex == -1) { Debug.Log("Racer is not in Race Manager"); return -1; }

        int currentProgress = racersProgress[racerIndex];

        int currentPosition = 0;
        foreach(int racerProgress in racersProgress)
        {
            if(racerProgress >= currentProgress) { currentPosition += 1; }
        }
        
        return currentPosition;
    }

    // Should compute Player Position here instead of in player script

    public void StartCountdown()
    {
        foreach (GameObject racer in racersArray)
        {
            racer.GetComponent<Vehicle>().SetNoEngine(true);
        }
        myAnimator.SetTrigger("StartCountdown");
    }

    public void StartRace()
    {
        foreach(GameObject racer in racersArray)
        {
            racer.GetComponent<Vehicle>().SetNoEngine(false);
        }
    }
}