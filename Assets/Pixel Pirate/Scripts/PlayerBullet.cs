using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    const string LAYER_DESTROYER = "PP_Destroyer";

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Destroy(gameObject);
    }

    public void MoveBullet(float horizontalSpeed, float verticalSpeed)
    {
        Rigidbody2D myRigidBody = GetComponent<Rigidbody2D>();
        Vector2 bulletVelocity = new Vector2(horizontalSpeed, verticalSpeed);
        myRigidBody.velocity = bulletVelocity;
    }
}