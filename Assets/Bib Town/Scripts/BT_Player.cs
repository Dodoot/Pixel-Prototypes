using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Player : MonoBehaviour
{
    [SerializeField] float walkSpeedVertical = 3f;
    [SerializeField] float walkSpeedHorizontal = 4f;

    public bool isTalking = false;
    BT_Talker currentTalker = null;

    Animator myAnimator;
    Rigidbody2D myRigidbody;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();

        if (!myRigidbody) { Debug.Log("No Rigidbody attached to player"); }
        if (!myAnimator) { Debug.Log("No Animator attached to player"); }
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!isTalking)
        {
            Move();
            Talk();
        }
    }

    public void setCurrentTalker(BT_Talker talker)
    {
        currentTalker = talker;
    }

    private void Talk()
    {
        if (Input.GetButtonDown("Action1"))
        {
            if (currentTalker)
            {
                currentTalker.Talk();
                StopMovingAndFaceTalker();
            }
        }
    }

    private void StopMovingAndFaceTalker()
    {
        myRigidbody.velocity = Vector2.zero;

        myAnimator.SetBool("Walk Side", false);
        myAnimator.SetBool("Walk Top", false);
        myAnimator.SetBool("Walk Down", false);

        Vector2 talkerRelativePosition = transform.InverseTransformPoint(currentTalker.transform.position);
        if(Mathf.Abs(talkerRelativePosition.x)> Mathf.Abs(talkerRelativePosition.y))
        {
            myAnimator.SetTrigger("Idle Side");
            Turn(talkerRelativePosition.x * transform.localScale.x);
        }
        else
        {
            if(talkerRelativePosition.y > 0)
            {
                myAnimator.SetTrigger("Idle Top");
            }
            else
            {
                myAnimator.SetTrigger("Idle Down");
            }
        }
    }

    private void Move()
    {
        float controlThrowHorizontal = Input.GetAxis("Horizontal");
        float controlThrowVertical = Input.GetAxis("Vertical");

        float playerVelocityX = 0;
        float playerVelocityY = 0;

        string walkDirection = null;

        if (Mathf.Abs(controlThrowHorizontal) > 0.1f)
        {
            playerVelocityX = controlThrowHorizontal * walkSpeedHorizontal;

            Turn(controlThrowHorizontal);
            walkDirection = "Side";
        }

        if (Mathf.Abs(controlThrowVertical) > 0.1f)
        {
            playerVelocityY = controlThrowVertical * walkSpeedVertical;

            if (Mathf.Abs(controlThrowVertical) > Mathf.Abs(controlThrowHorizontal) * walkSpeedHorizontal / walkSpeedVertical)
            {
                if (controlThrowVertical > Mathf.Epsilon)
                {
                    walkDirection = "Top";
                }
                if (controlThrowVertical < -Mathf.Epsilon)
                {
                    walkDirection = "Down";
                }
            }
        }

        myAnimator.SetBool("Walk Side", false);
        myAnimator.SetBool("Walk Top", false);
        myAnimator.SetBool("Walk Down", false);

        if (walkDirection != null)
        {
            myAnimator.SetBool("Walk " + walkDirection, true);
        }

        Vector2 playerVelocity = new Vector2(playerVelocityX, playerVelocityY);
        myRigidbody.velocity = playerVelocity;
    }

    public void Turn(float direction)
    /// negative for left, positive for 
    {
        if (direction < 0)
        {
            Vector3 newScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            transform.localScale = newScale;
        }
        if (direction > 0)
        {
            Vector3 newScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            transform.localScale = newScale;
        }
    }
}
