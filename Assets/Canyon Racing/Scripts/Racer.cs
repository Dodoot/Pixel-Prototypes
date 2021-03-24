using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racer : MonoBehaviour
{
    Vehicle myVehicle;
    CheckpointManager myCheckpointManager;
    RaceManager myRaceManager;

    int checkpointIndex;
    GameObject checkpoint;
    GameObject lastCheckpoint;
    int turnsCompleted;
    int racePosition;

    public GameObject GetCheckpoint() { return checkpoint; }
    public GameObject GetLastCheckpoint() { return lastCheckpoint; }

    private void Start()
    {
        myVehicle = GetComponent<Vehicle>();
        myCheckpointManager = FindObjectOfType<CheckpointManager>();
        myRaceManager = FindObjectOfType<RaceManager>();

        checkpointIndex = 0;
        checkpoint = myCheckpointManager.GetCheckpointByIndex(checkpointIndex);
        lastCheckpoint = checkpoint;

        turnsCompleted = 0;
        racePosition = 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == checkpoint)
        {
            Debug.Log("reached Objective");
            lastCheckpoint = checkpoint;

            checkpointIndex += 1;
            checkpoint = myCheckpointManager.GetCheckpointByIndex(checkpointIndex);
            if (!checkpoint)
            {
                checkpointIndex = 0;
                checkpoint = myCheckpointManager.GetCheckpointByIndex(checkpointIndex);

                turnsCompleted += 1;
            }
        }

        myRaceManager.UpdateProgress(gameObject, turnsCompleted, checkpointIndex);
        racePosition = myRaceManager.GetPosition(gameObject);
    }

    public void SetNoEngineOnVehicle(bool newNoEngine)
    {
        myVehicle.SetNoEngine(newNoEngine);
    }
}
