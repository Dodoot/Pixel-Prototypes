using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] GameObject[] checkpointArray = null;

    public GameObject GetCheckpointByIndex (int CheckpointIndex)
    {
        if (CheckpointIndex >= checkpointArray.Length) { return null; }
        else { return checkpointArray[CheckpointIndex]; }
    }

    public int GetNumberOfCheckpoints()
    {
        return checkpointArray.Length;
    }
}