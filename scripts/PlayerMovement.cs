using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator anim;
    private SpriteRenderer sprite;
    private BoxCollider2D coll;

    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 13f;

    [SerializeField] private AudioSource jumpSoundEffect;
    
    

    private void Start()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        body = GetComponent<Rigidbody2D>();
        if (body == null)
        {
            body = gameObject.AddComponent<Rigidbody2D>();
        }

        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        if (jumpSoundEffect == null)
        {
            Debug.LogError("Jump sound effect is not assigned to the player.");
        }
    }

    private void Update()
    {
        HandleMovementInput();
        UpdateAnimationState();
    }

    private void HandleMovementInput()
    {
        float dirx = Input.GetAxisRaw("Horizontal");
        body.velocity = new Vector2(dirx * moveSpeed, body.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();
        }
    }

    private void Jump()
    {
        jumpSoundEffect.Play();
        body.velocity = new Vector2(body.velocity.x, jumpForce);
    }

    private void UpdateAnimationState()
    {
        if (body == null)
        {
            Debug.LogError("Rigidbody2D component is missing on the player GameObject.");
            return;
        }

        MovementState state;

        if (Mathf.Abs(body.velocity.x) > 0.1f)
        {
            state = MovementState.running;
            sprite.flipX = body.velocity.x < 0f;
        }
        else
        {
            state = MovementState.idle;
        }

        if (Mathf.Abs(body.velocity.y) > 0.1f)
        {
            state = body.velocity.y > 0 ? MovementState.jumping : MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
    }

    private enum MovementState { idle, running, jumping, falling }
}
