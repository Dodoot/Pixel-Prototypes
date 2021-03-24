using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMouette : MonoBehaviour
{
    const float SCREEN_VERTICAL_SIZE = 5f;
    const float SCREEN_HORIZONTAL_SIZE = 10;

    const string LAYER_PLAYER_PROJECTILE = "PP_Player Projectile";
    const string LAYER_DESTROYER = "PP_Destroyer";
    const string LAYER_CRATE_TRIGGER = "PP_Explosive Crate Trigger";

    [SerializeField] float movementSpeed = 3f;
    [SerializeField] int direction = 1;

    [SerializeField] int scoreValue = 1;

    [SerializeField] EnemyBomb bombPrefab = null;
    [SerializeField] GameObject outOfFrameIndicator = null;

    float lastXPos;

    Camera myCamera;

    void Start()
    {
        setDirection(direction);
        lastXPos = transform.position.x;

        myCamera = FindObjectOfType<Camera>();
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        // quite ugly to recompute this in every enemy update of every mouette
        float cameraXNegLim = myCamera.transform.position.x - SCREEN_HORIZONTAL_SIZE;
        float cameraXPosLim = myCamera.transform.position.x + SCREEN_HORIZONTAL_SIZE;
        float cameraYNegLim = myCamera.transform.position.y - SCREEN_VERTICAL_SIZE;
        float cameraYPosLim = myCamera.transform.position.y + SCREEN_VERTICAL_SIZE;

        bool outOfFrame = (transform.position.x < cameraXNegLim
                           || transform.position.x > cameraXPosLim
                           || transform.position.y < cameraYNegLim
                           || transform.position.y > cameraYPosLim);

        if (outOfFrame)
        {
            Vector2 newIndicatorPosition = new Vector2(
                Mathf.Min(cameraXPosLim, Mathf.Max(cameraXNegLim, transform.position.x)),
                Mathf.Min(cameraYPosLim, Mathf.Max(cameraYNegLim, transform.position.y - 0.3f))); // ouch ugly ugly
            outOfFrameIndicator.transform.position = newIndicatorPosition;
            outOfFrameIndicator.SetActive(true);
        }
        else
        {
            outOfFrameIndicator.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer(LAYER_PLAYER_PROJECTILE))
        {
            DieByProjectile();
        }

        if (collider.gameObject.layer == LayerMask.NameToLayer(LAYER_DESTROYER))
        {
            DieByDestroyer();
        }

        if (collider.gameObject.layer == LayerMask.NameToLayer(LAYER_CRATE_TRIGGER))
        {
            Attack();
        }
    }

    public void setDirection(int newDirection)
    {
        direction = newDirection;

        transform.localScale = new Vector2(direction, transform.localScale.y);

        float signedSpeed = direction * movementSpeed;
        Vector2 velocity = new Vector2(signedSpeed, 0);
        GetComponent<Rigidbody2D>().velocity = velocity;
    }

    private void Attack()
    {
        StartCoroutine(WaitBeforeAttack());
    }

    IEnumerator WaitBeforeAttack()
    {
        yield return new WaitForSeconds(0.2f);
        Instantiate(bombPrefab, transform.position, Quaternion.identity);
    }

    private void DieByProjectile()
    {
        MusicAndSoundManager.PlaySound("Mouette", transform.position);
        FindObjectOfType<ScoreController>().AddToScore(scoreValue);
        Destroy(gameObject);
    }

    private void DieByDestroyer()
    {
        Destroy(gameObject);
    }
}
