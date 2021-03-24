using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    [Header("Vehicle Physics")]
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float accelerationForce = 10f;
    [SerializeField] float naturalBrakeRatio = 0.95f;
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float fallForce = 5f;

    bool inputAccelerate = false;
    bool inputBrake = false;
    bool inputRight = false;
    bool inputLeft = false;

    bool isFalling = false;
    bool noEngine = false;

    float currentAngle;
    Vector2 currentDirection;

    Collider2D colliderRight = null;
    Collider2D colliderLeft = null;

    Racer myRacer;
    Rigidbody2D myRigidbody2D;
    Animator myAnimator;
    AudioSource myAudioSource;

    // get and set
    public void SetInputAccelerate(bool newInputAccelerate) { inputAccelerate = newInputAccelerate; }
    public void SetInputBrake(bool newInputBrake) { inputBrake = newInputBrake; }
    public void SetInputRight(bool newInputRight) { inputRight = newInputRight; }
    public void SetInputLeft(bool newInputLeft) { inputLeft = newInputLeft; }

    public void SetNoEngine(bool newNoEngine) { noEngine = newNoEngine; }

    public void SetColliderRight(Collider2D newColliderRight)
    {
        colliderRight = newColliderRight;
    }
    public void SetColliderLeft(Collider2D newColliderLeft)
    {
        colliderLeft = newColliderLeft;
    }

    // Unity Methods
    void Start()
    {
        isFalling = false;
        currentAngle = 0;
        currentDirection = new Vector2(0, 1);

        myRacer = GetComponent<Racer>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        if (!isFalling)
        {
            if (!noEngine)
            {
                ApplyRotation();
                ApplyAcceleration();
            }
            CheckFall();
        }
        if (myAudioSource)
        {
            UpdatePitchAndVolume();
        }
    }

    private void UpdatePitchAndVolume()
    {
        if (isFalling)
        {
            myAudioSource.mute = true;
        }
        else
        {
            myAudioSource.mute = false;
            float currentSpeed = myRigidbody2D.velocity.magnitude;
            float speedRatio = currentSpeed / maxSpeed;
            float newPitch = 1.5f * speedRatio;
            myAudioSource.pitch = newPitch;
        }
    }

    private void UpdateAnimator()
    {
        if (inputAccelerate && !noEngine) { myAnimator.SetBool("active", true); }
        else { myAnimator.SetBool("active", false); }
    }

    private void ApplyRotation()
    {
        if (inputRight)
        {
            transform.Rotate(0, 0, -rotationSpeed);
        }
        else if (inputLeft)
        {
            transform.Rotate(0, 0, rotationSpeed);
        }
    }

    private void ApplyAcceleration()
    {
        currentAngle = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        currentDirection = new Vector2(
            -Mathf.Sin(currentAngle),
            Mathf.Cos(currentAngle));

        if (inputAccelerate)
        {
            myRigidbody2D.AddForce(currentDirection * accelerationForce);
        }

        myRigidbody2D.velocity = myRigidbody2D.velocity * naturalBrakeRatio;

        if (myRigidbody2D.velocity.magnitude > maxSpeed)
        {
            // Debug.Log("Too FAST");
        }
    }

    private void CheckFall()
    {
        if (colliderRight)
        {
            float rightAngle = currentAngle - Mathf.PI / 2f;
            Vector2 rightDirection = new Vector2(
                -Mathf.Sin(rightAngle),
                Mathf.Cos(rightAngle));
            myRigidbody2D.AddForce(rightDirection * fallForce);
        }

        if (colliderLeft)
        {
            float leftAngle = currentAngle + Mathf.PI / 2f;
            Vector2 leftDirection = new Vector2(
                -Mathf.Sin(leftAngle),
                Mathf.Cos(leftAngle));
            myRigidbody2D.AddForce(leftDirection * fallForce);
        }

        if (colliderRight && colliderLeft == colliderRight)
        {
            Fall();
        }
    }

    private void Fall()
    {
        isFalling = true;
        myRigidbody2D.velocity = Vector2.zero;
        myAnimator.SetTrigger("Fall");
        // The animation is going to respawn when it ends. Good idea or should I use a WaitForSeconds ?
    }

    public void Respawn()
    {
        Vector2 respawnPosition = myRacer.GetLastCheckpoint().transform.position;
        Quaternion respawnRotation = myRacer.GetLastCheckpoint().transform.rotation;
        transform.position = respawnPosition;
        transform.rotation = respawnRotation;
        isFalling = false;
        colliderLeft = null;
        colliderRight = null;
    }
}

