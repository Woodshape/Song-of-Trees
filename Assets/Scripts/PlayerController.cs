using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityController
{
    public float JumpForce;
    public int NumberOfJumps;

    public float ClimbSpeed;

    private Animator myAnimator;

    private int extraJumps;

    private float climbInput;

    public bool isAtLadder = false;
    public bool isOnLadder = false;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    protected override void Start()
    {
        base.Start();

        myAnimator = GetComponent<Animator>();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void Update()
    {
        if (entityState == EnityStates.STAGGER)
            return;

        moveInput = Input.GetAxisRaw("Horizontal");

        climbInput = Input.GetAxisRaw("Vertical");

        if (isGrounded)
        {
            extraJumps = NumberOfJumps - 1;
        }

        if (isAtLadder && climbInput != 0)
        {
            isOnLadder = true;
            isGrounded = false;
            // myRigidbody.velocity = Vector2.up;
            if (climbInput > 0)
            {
                myRigidbody.velocity = Vector2.up * ClimbSpeed;
            }
            else
            {
                myRigidbody.velocity = Vector2.down * ClimbSpeed;
            }
        }
        else if (isOnLadder)
        {
            myRigidbody.Sleep();
        }

        if (Input.GetButtonDown("Jump") && extraJumps > 0 && !isOnLadder)
        {
            // We have additional jumps, so it doesn't matter if we are airbourne
            myRigidbody.velocity = Vector2.up * JumpForce;
            extraJumps--;
        }
        else if (Input.GetButtonDown("Jump") && extraJumps == 0 && isGrounded)
        {
            // No extra jumps - basic jump mechanic -, so we have to me sure we are on the ground
            myRigidbody.velocity = Vector2.up * JumpForce;
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // protected override void FixedUpdate()
    // {
    //     if (playerState != PlayerStates.STAGGER)
    //     {
    //         base.FixedUpdate();
    //     }
    // }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void HandleAttackAnimation()
    {
        myAnimator.SetTrigger("tAttack");
    }
}

// public enum PlayerStates
// {
//     STAGGER,
//     MOVE,
//     ATTACK
// }
