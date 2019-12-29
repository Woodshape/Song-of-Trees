using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EntityController
{
    // [SerializeField]
    // private EnemyStates enemyState;

    public Transform WallCheck;

    private float timeBetweenTurns;

    private bool isAtWall = false;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    protected override void Start()
    {
        base.Start();

        // enemyState = EnemyStates.MOVE;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<Entity>().IsAlive)
            return;
        // if (enemyState != EnemyStates.STAGGER)
        // {
        //     Patrol();
        // }

        Patrol();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // protected override void FixedUpdate()
    // {
    //     if (enemyState != EnemyStates.STAGGER)
    //     {
    //         base.FixedUpdate();
    //     }
    // }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void Patrol()
    {
        isAtWall = Physics2D.OverlapCircle(WallCheck.position, CheckRadius, GroundLayer);

        if (moveInput == 0)
        {
            int[] moves = new int[] { -1, 1 };
            int index = Random.Range(0, moves.Length - 1);
            moveInput = moves[index];
        }

        if ((!isGrounded || isAtWall) && timeBetweenTurns <= 0)
        {
            moveInput = -moveInput;
            timeBetweenTurns = 0.25f;
        }
        else
        {
            timeBetweenTurns -= Time.deltaTime;
        }
    }
}

// public enum EnemyStates
// {
//     STAGGER,
//     MOVE,
//     ATTACK
// }
