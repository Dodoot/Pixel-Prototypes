using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomb : MonoBehaviour
{
    [SerializeField] float bombSpeedY = -3;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D myRigidBody = GetComponent<Rigidbody2D>();
        Vector2 bombVelocity = new Vector2(0, bombSpeedY);
        myRigidBody.velocity = bombVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Destroy(gameObject);
    }
}
