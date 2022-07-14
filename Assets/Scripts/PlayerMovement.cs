using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 2f;
    [SerializeField] Vector2 deathThrow = new Vector2(15f, 15f);
    [SerializeField] GameObject arrow;
    [SerializeField] Transform gun;

    PlayerInput playerInput;

    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    BoxCollider2D myBodyCollider;
    CapsuleCollider2D myFeetCollider;
    float startingGravity;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<BoxCollider2D>();
        myFeetCollider = GetComponent<CapsuleCollider2D>();
        playerInput = GetComponent<PlayerInput>();

        startingGravity = myRigidbody.gravityScale;
        
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        FlipSprite();
        ClimbLadder();
    }

    void OnFire(InputValue value)
    {
        Instantiate(arrow, gun.position, transform.rotation);
        myAnimator.SetTrigger("Shoot");
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {

        if(value.isPressed && myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
            myAnimator.SetBool("isJumping", true);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground" || collision.gameObject.tag=="Ladder")
        {
            myAnimator.SetBool("isJumping", false);
        }



        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            Die();
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
       
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
        
        
    }

    void ClimbLadder()
    {
        if(myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            Debug.Log("Touching ladder");

            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbSpeed);
            myRigidbody.gravityScale = 0f;
            bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;

            myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
        } else
        {
            myRigidbody.gravityScale = startingGravity;
            myAnimator.SetBool("isClimbing", false);
        }
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
             

    }

    void Die()
    {
        playerInput.enabled = false;
        myAnimator.SetTrigger("Ded");
        myRigidbody.velocity = deathThrow;
    }
}
