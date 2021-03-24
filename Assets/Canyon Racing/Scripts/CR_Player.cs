using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CR_Player : MonoBehaviour
{
    Racer myRacer;
    Vehicle myVehicle;

    void Start()
    {
        myRacer = GetComponent<Racer>();
        myVehicle = GetComponent<Vehicle>();
    }
    
    void Update()
    {
        CheckInputs();
    }

    private void CheckInputs()
    {
        // myVehicle.SetInputAccelerate(Input.GetKey("up"));
        myVehicle.SetInputAccelerate(true); // always accelerating
        myVehicle.SetInputBrake(Input.GetKey("down"));
        myVehicle.SetInputLeft(Input.GetKey("left"));
        myVehicle.SetInputRight(Input.GetKey("right"));
    }
}
