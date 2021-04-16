using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
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
        Move();
    }

    void Move()
    {
        myRigidBody.velocity = new Vector2(moveSpeed, 0f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        moveSpeed = -moveSpeed;
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        // myRenderer.flipX = !myRenderer.flipX;
        // if (myRigidBody.velocity.x > 0) myRenderer.flipX = false;
        // else if (myRigidBody.velocity.x < 0) myRenderer.flipX = true;
        //transform.localScale = new Vector3(Mathf.Sign(myRigidBody.velocity.x), 1f);
    }
}
