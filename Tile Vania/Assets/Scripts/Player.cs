using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;

    Rigidbody2D myRigidBody;
    SpriteRenderer myRenderer;
    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        FlipSprite();
    }

    private void Run()
    {
        var deltaX = Input.GetAxis("Horizontal") * runSpeed;

        Vector2 playerVelocity = new Vector2(deltaX, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
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
