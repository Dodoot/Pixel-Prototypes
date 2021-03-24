using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("AI")]
    [SerializeField] float minDeltaAngleToTurn = 0.1f;

    Vehicle myVehicle;
    CheckpointManager myCheckpointManager;
    RaceManager myRaceManager;

    int checkpointIndex;
    GameObject checkpoint;
    int turnsCompleted;

    Vector2 objectiveVector;
    float currentAngle;
    float objectiveAngle;
    float deltaAngle;

    void Start()
    {
        myVehicle = GetComponent<Vehicle>();
        myCheckpointManager = FindObjectOfType<CheckpointManager>();
        myRaceManager = FindObjectOfType<RaceManager>();

        checkpointIndex = 0;
        checkpoint = myCheckpointManager.GetCheckpointByIndex(checkpointIndex);

        turnsCompleted = 0;
    }

    void Update()
    {
        DecideInputs();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == checkpoint)
        {
            Debug.Log("reached Objective");
            checkpointIndex += 1;
            checkpoint = myCheckpointManager.GetCheckpointByIndex(checkpointIndex);
            if (!checkpoint)
            {
                checkpointIndex = 0;
                checkpoint = myCheckpointManager.GetCheckpointByIndex(checkpointIndex);

                turnsCompleted += 1;
            }

            myRaceManager.UpdateProgress(gameObject, turnsCompleted, checkpointIndex);
        }
    }

    private void DecideInputs()
    {
        currentAngle = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        objectiveVector = (Vector2)checkpoint.transform.position - (Vector2)transform.position;
        objectiveAngle = Vector2.SignedAngle(new Vector2(0, 1), objectiveVector) * Mathf.Deg2Rad;
        deltaAngle = objectiveAngle - currentAngle;
        if (deltaAngle < Mathf.PI) { deltaAngle = deltaAngle + 2 * Mathf.PI; }
        if (deltaAngle > Mathf.PI) { deltaAngle = deltaAngle - 2 * Mathf.PI; }

        // Debug.Log("---");
        // Debug.Log(objectiveVector);
        // Debug.Log(deltaAngle);

        myVehicle.SetInputLeft(deltaAngle > minDeltaAngleToTurn);
        myVehicle.SetInputRight(deltaAngle < -minDeltaAngleToTurn);

        myVehicle.SetInputAccelerate(true);
    }
}
