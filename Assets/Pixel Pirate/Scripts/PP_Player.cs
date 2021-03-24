using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class PP_Player : MonoBehaviour
{
    const string LAYER_GROUND = "PP_Ground";
    const string LAYER_PLATFORMS = "PP_Platforms";
    const string LAYER_PLAYER = "PP_Player";
    const string LAYER_ENEMY = "PP_Enemy";
    const string LAYER_ENEMY_PROJECTILE = "PP_Enemy Projectile";
    const string LAYER_DESTROYER = "PP_Destroyer";

    // Config Params
    [SerializeField] float runSpeed = 4f;
    [SerializeField] int maxBullets = 6;
    [SerializeField] float offsetBulletY = -0.15f;

    [Header("Jump")]
    [SerializeField] float jumpForce = 8f;
    [SerializeField] float bufferTime = 0.2f;
    [SerializeField] float maxHoldTime = 0.5f;
    [SerializeField] float holdForce = 4f;
    [SerializeField] float maxHoldBTime = 0.5f;
    [SerializeField] float holdBForce = 4f;
    [SerializeField] float maxJumpSpeed = 6f;
    [SerializeField] float maxFallSpeed = 6f;

    [Header("Air Jumps")]
    [SerializeField] int airJumps = 1;
    [SerializeField] float airJumpForce = 6f;

    [Header("Projectile Attack")]
    [SerializeField] GameObject projectilePrefab = null;
    [SerializeField] float projectileSpeed = 10f;

    [Header("UI")]
    [SerializeField] GameObject[] ammoImages = null;
    [SerializeField] GameObject noAmmoImage = null;

    [Header("References")]
    [SerializeField] Collider2D myBodyCollider = null;
    [SerializeField] Collider2D myFeetCollider = null;
    [SerializeField] Collider2D myCoyoteCollider = null;

    // Variables and Cached References
    Rigidbody2D myRigidBody;
    Animator myAnimator;

    int numberBullets;

    bool bufferedJump = false;
    bool jumpHeld = false;
    bool canHold = false;
    float bufferedJumpTime = 0f;
    float jumpHoldTime = 0;
    int airJumpsLeft;
    bool canGroundJump;

    public int GetNumberBullets() { return numberBullets; }

    // Gets and Sets
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();

        if (!myRigidBody) { Debug.Log("No Rigid Body attached to player"); }
        if (!myAnimator) { Debug.Log("No Animator attached to player"); }
        if (!myFeetCollider) { Debug.Log("No feet Collider attached to player"); }
        if (!myBodyCollider) { Debug.Log("No body Collider attached to player"); }

        numberBullets = maxBullets;
        RefreshAmmoUI();
    }

    void Update()
    {
        bool isFeetTouchingGround = myFeetCollider.IsTouchingLayers(LayerMask.GetMask(LAYER_GROUND));
        bool isFeetTouchingPlatforms = myFeetCollider.IsTouchingLayers(LayerMask.GetMask(LAYER_PLATFORMS));
        if (isFeetTouchingGround | isFeetTouchingPlatforms)
        {
            airJumpsLeft = airJumps;
            canGroundJump = true;
        }

        checkJumpInputs();

        FallThroughPlatforms();
        Attack();
    }

    private void FixedUpdate()
    {
        Move();
        Jump();

        JumpHold();
        AirSpeedLimit();
    }

    public void ReloadAmmo()
    {
        MusicAndSoundManager.PlaySound("Ammo", transform.position);
        numberBullets = maxBullets;
        RefreshAmmoUI();
    }

    private void RefreshAmmoUI()
    {
        for(int i=0; i < ammoImages.Length; i++)
        {
            if (i < numberBullets)
            {
                ammoImages[i].SetActive(true);
            }
            else
            {
                ammoImages[i].SetActive(false);
            }
        }
        if(numberBullets <= 0)
        {
            noAmmoImage.SetActive(true);
        }
        else
        {
            noAmmoImage.SetActive(false);
        }
    }

    private void checkJumpInputs()
    {
        if (bufferedJump)
        {
            bufferedJumpTime += Time.deltaTime;
            if (bufferedJumpTime >= bufferTime)
            {
                bufferedJump = false;
            }
        }

        if (Input.GetButtonDown("PP_Jump"))
        {
            bufferedJump = true;
            bufferedJumpTime = 0f;
        }

        if (Input.GetButton("PP_Jump"))
        {
            jumpHeld = true;
        }
        else
        {
            jumpHeld = false;
        }
    }

    private void Move()
    {
        float controlThrow = Input.GetAxis("Horizontal");

        if (Mathf.Abs(controlThrow) > 0.1f)
        {
            Turn(controlThrow);

            Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
            myRigidBody.velocity = playerVelocity;

            myAnimator.SetBool("Running", true);
        }
        else
        {
            Vector2 playerVelocity = new Vector2(0, myRigidBody.velocity.y);
            myRigidBody.velocity = playerVelocity;

            myAnimator.SetBool("Running", false);
        }
    }

    private void Jump()
    {
        bool isFeetTouchingGround = myFeetCollider.IsTouchingLayers(LayerMask.GetMask(LAYER_GROUND));
        bool isFeetTouchingPlatforms = myFeetCollider.IsTouchingLayers(LayerMask.GetMask(LAYER_PLATFORMS));
        bool isCoyoteTouchingGround = myCoyoteCollider.IsTouchingLayers(LayerMask.GetMask(LAYER_GROUND));
        bool isCoyoteTouchingPlatforms = myCoyoteCollider.IsTouchingLayers(LayerMask.GetMask(LAYER_PLATFORMS));

        bool isPlayerToutchingGroundOrPlatforms = (isFeetTouchingGround || isFeetTouchingPlatforms || isCoyoteTouchingGround || isCoyoteTouchingPlatforms);
        if (!isPlayerToutchingGroundOrPlatforms) { canGroundJump = false; }

        if (bufferedJump)
        {
            if (canGroundJump)
            {
                // MusicAndSoundManager.PlaySound("Woosh");
                myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, 0);
                myRigidBody.AddForce(Vector2.up * jumpForce);
                bufferedJump = false;
                canHold = true;
                jumpHoldTime = 0f;
            }
            else if (airJumpsLeft > 0)
            {
                // MusicAndSoundManager.PlaySound("Woosh");
                myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, 0);
                myRigidBody.AddForce(Vector2.up * airJumpForce);
                airJumpsLeft -= 1;
                bufferedJump = false;
                canHold = true;
                jumpHoldTime = 0f;
            }
        }
    }

    private void JumpHold()
    {
        if (canHold && jumpHeld)
        {
            jumpHoldTime += Time.deltaTime;
            if (jumpHoldTime <= maxHoldTime)
            {
                myRigidBody.AddForce(Vector2.up * holdForce);
            }
            else if (jumpHoldTime <= maxHoldBTime)
            {
                myRigidBody.AddForce(Vector2.up * holdBForce);
            }
            else
            {
                canHold = false;
            }
        }
        else
        {
            canHold = false;
        }
    }

    private void AirSpeedLimit()
    {
        if (myRigidBody.velocity.y > maxJumpSpeed)
        {
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, maxJumpSpeed);
        }
        if (myRigidBody.velocity.y < -maxFallSpeed)
        {
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, -maxFallSpeed);
        }
    }

    private void Attack()
    {
        if (Input.GetButtonDown("Action1") || Input.GetButtonDown("Action2"))
        {
            if(numberBullets > 0)
            {
                MusicAndSoundManager.PlaySound("Gun", transform.position);
                GameObject projectile = Instantiate(
                    projectilePrefab,
                    new Vector2(transform.position.x, transform.position.y + offsetBulletY),
                    Quaternion.identity);

                projectile.GetComponent<PlayerBullet>().MoveBullet(transform.localScale.x * projectileSpeed, 0);

                numberBullets -= 1;
                RefreshAmmoUI();
            }
            else
            {
                MusicAndSoundManager.PlaySound("Click", transform.position);
            }
        }
    }

    private void FallThroughPlatforms()
    {
        if (Input.GetAxis("Vertical") < -0.3f)
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(LAYER_PLATFORMS), LayerMask.NameToLayer(LAYER_PLAYER), true);
        }
        else
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(LAYER_PLATFORMS), LayerMask.NameToLayer(LAYER_PLAYER), false);
        }
    }

    public void Turn(float direction)
    /// negative for left, positive for right
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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer(LAYER_ENEMY)
            || collider.gameObject.layer == LayerMask.NameToLayer(LAYER_ENEMY_PROJECTILE))
        {
            Hurt();
        }

        if (collider.gameObject.layer == LayerMask.NameToLayer(LAYER_DESTROYER))
        {
            FindObjectOfType<GameMaster>().EndGame();
            // transform.position = Vector3.zero;
            // FindObjectOfType<Camera>().transform.position = Vector3.zero;
        }
    }

    public void Hurt()
    {
        // Debug.Log("OUCH");
    }
}
