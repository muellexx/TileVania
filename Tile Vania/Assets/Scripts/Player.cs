using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Config
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;

    // State
    bool isAlive = true;

    // Cached References
    Rigidbody2D myRigidBody;
    SpriteRenderer myRenderer;
    Animator myAnimator;
    Collider2D myCollider;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
        myCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        Jump();
        FlipSprite();
    }

    private void Run()
    {
        var deltaX = Input.GetAxis("Horizontal") * runSpeed;

        Vector2 playerVelocity = new Vector2(deltaX, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
        
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", playerHasHorizontalSpeed);
    }

    private void Jump()
    {
        if (!myCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) return;
        if (Input.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;
        }
        
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
}
