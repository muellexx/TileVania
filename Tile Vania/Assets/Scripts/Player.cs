using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Config
    [SerializeField] float walkSpeed = 7f;
    [SerializeField] float runSpeed = 12f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float jumpTimer = 2f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(25f, 25f);

    [SerializeField] AudioClip deathSound;
    [Range(0f, 1f)][SerializeField] float deathVolume = 1f;

    // State
    bool isAlive = true;
    float jumpHoldTime;

    // Cached References
    Rigidbody2D myRigidBody;
    SpriteRenderer myRenderer;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeet;

    // GameSession gameSession;
    float gravityScaleAtStart;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeet = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidBody.gravityScale;
        // gameSession = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAlive) return;
        Run();
        Climb();
        Jump();
        FlipSprite();
        EnemyCollision();
        HazardCollision();
    }

    private void Run()
    {
        var deltaX = Input.GetAxis("Horizontal");
        if(Input.GetButton("Run"))
        {
            deltaX *= runSpeed;
            myAnimator.SetBool("Running", true);
            myAnimator.SetBool("Walking", false);
        }
        else
        {
            deltaX *= walkSpeed;
            myAnimator.SetBool("Walking", true);
            myAnimator.SetBool("Running", false);
        }

        Vector2 playerVelocity = new Vector2(deltaX, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
        
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (!playerHasHorizontalSpeed)
        {
            myAnimator.SetBool("Walking", false);
            myAnimator.SetBool("Running", false);
        }
    }

    private void Jump()
    {
        // if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"))&&
        //     !myFeet.IsTouchingLayers(LayerMask.GetMask("Ladder"))) return;
        if (Input.GetButtonDown("Jump")&&
            myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"))||
            (myFeet.IsTouchingLayers(LayerMask.GetMask("Ladder"))&&
            !myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"))))
        {
            jumpHoldTime = Time.timeSinceLevelLoad;
        }
        if (Input.GetButton("Jump"))
        {
            if (Time.timeSinceLevelLoad - jumpHoldTime <= jumpTimer)
            {
                Vector2 jumpVelocity = new Vector2(myRigidBody.velocity.x, jumpSpeed);
                myRigidBody.velocity = jumpVelocity;
            }
        }
        
    }

    private void Climb()
    {
        if (!myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            myAnimator.SetBool("Climbing", false);
            if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Ladder")))
            {
                myRigidBody.gravityScale = gravityScaleAtStart;
                myAnimator.SetBool("OnLadder", false);
            }
            return;
        }
        myAnimator.SetBool("OnLadder", true);
        var deltaY = Input.GetAxis("Vertical") * climbSpeed;

        Vector2 playerVelocity = new Vector2(myRigidBody.velocity.x, deltaY);
        myRigidBody.velocity = playerVelocity;
        myRigidBody.gravityScale = 0;

        bool playerIsClimbing = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
        if (playerIsClimbing)
        {
            myAnimator.SetBool("Climbing", true);
            myAnimator.SetBool("Walking", false);
            myAnimator.SetBool("Running", false);
        }
    }

    private void EnemyCollision()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            StartCoroutine(Die());
        }
    }

    private void HazardCollision()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Hazard")))
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        myAnimator.SetTrigger("Dying");
        myRigidBody.velocity = deathKick;
        PhysicsMaterial2D M = Instantiate(myBodyCollider.sharedMaterial);
        M.friction = 1;
        myBodyCollider.sharedMaterial = M;
        myRigidBody.gravityScale = gravityScaleAtStart;
        isAlive = false;
        if (deathSound != null)
        {
            GameObject audioListener = GameObject.FindWithTag("AudioListener");
            AudioSource.PlayClipAtPoint(
                deathSound,
                audioListener.transform.position,
                deathVolume);
        }
        yield return new WaitForSeconds(2);
        GameSession gameSession = FindObjectOfType<GameSession>();
        gameSession.ProcessPlayerDeath();
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            if (myRigidBody.velocity.x > 0) myRenderer.flipX = false;
            else if (myRigidBody.velocity.x < 0) myRenderer.flipX = true;
            //transform.localScale = new Vector3(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }

    public void StopMovement()
    {
        isAlive = false;
        myAnimator.SetBool("Climbing", false);
        myAnimator.SetBool("Walking", false);
        myAnimator.SetBool("Running", false);
    }
}
