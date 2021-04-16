using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Config
    [SerializeField] float walkSpeed = 7f;
    [SerializeField] float runSpeed = 12f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(25f, 25f);

    // State
    bool isAlive = true;

    // Cached References
    Rigidbody2D myRigidBody;
    SpriteRenderer myRenderer;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeet;
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
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"))&&
            !myFeet.IsTouchingLayers(LayerMask.GetMask("Ladder"))) return;
        if (Input.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;
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
            }
            return;
        }
        var deltaY = Input.GetAxis("Vertical") * climbSpeed;

        Vector2 playerVelocity = new Vector2(myRigidBody.velocity.x, deltaY);
        myRigidBody.velocity = playerVelocity;
        myRigidBody.gravityScale = 0;

        bool playerIsClimbing = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("Climbing", playerIsClimbing);
    }

    private void EnemyCollision()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            Die();
        }
    }

    private void HazardCollision()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Hazard")))
        {
            Die();
        }
    }

    private void Die()
    {
        myAnimator.SetTrigger("Dying");
        myRigidBody.velocity = deathKick;
        PhysicsMaterial2D M = Instantiate(myBodyCollider.sharedMaterial);
        M.friction = 1;
        myBodyCollider.sharedMaterial = M;
        myRigidBody.gravityScale = gravityScaleAtStart;
        isAlive = false;
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

    private void onTriggerEnter2D()
    {

    }
}
