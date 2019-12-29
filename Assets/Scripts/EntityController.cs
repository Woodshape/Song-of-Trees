using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class EntityController : MonoBehaviour
{
    protected Rigidbody2D myRigidbody;

    public EnityStates entityState;

    public float Speed;

    public Transform GroundedCheck;
    public float CheckRadius;
    public LayerMask GroundLayer;

    protected float moveInput;
    protected bool isGrounded;
    protected bool isFacingRight = true;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Start is called before the first frame update
    protected virtual void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();

        entityState = EnityStates.MOVE;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        if (!GetComponent<Entity>().IsAlive || entityState == EnityStates.STAGGER)
            return;

        isGrounded = Physics2D.OverlapCircle(GroundedCheck.position, CheckRadius, GroundLayer);

        myRigidbody.velocity = new Vector2(moveInput * Speed, myRigidbody.velocity.y);

        if (!isFacingRight && moveInput > 0)
        {
            Flip();
        }
        else if (isFacingRight && moveInput < 0)
        {
            Flip();
        }
    }

    //TODO: Add flying behaviour

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public virtual void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void ChangeEnemyState(EnityStates newState)
    {
        if (entityState != newState)
        {
            entityState = newState;
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private float CalculateCollisionVectorFloat(Entity entity, Entity target)
    {
        float diff = target.transform.position.x - entity.transform.position.x;

        return diff;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public IEnumerator Flash()
    {
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sprite in sprites)
        {
            if (sprite.gameObject.name == "Body")
            {
                //Color spriteColor = sprite.color;

                for (int i = 0; i < 2; i++)
                {
                    //sprite.color = Color.red;
                    sprite.enabled = false;
                    yield return new WaitForSeconds(0.1f);
                    //sprite.color = spriteColor;
                    sprite.enabled = true;
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public IEnumerator Knockback(Entity knocker, Entity target, float knockbackForce, float knockbackTime)
    {
        ChangeEnemyState(EnityStates.STAGGER);

        float knockDir = CalculateCollisionVectorFloat(knocker, target);

        Rigidbody2D targetRigid = target.GetComponent<Rigidbody2D>();

        targetRigid.velocity = new Vector2(knockDir * knockbackForce, targetRigid.velocity.y + knockbackForce / 2);

        yield return new WaitForSeconds(knockbackTime);

        if (!targetRigid)
        {
            yield break;
        }

        targetRigid.velocity = Vector2.zero;

        ChangeEnemyState(EnityStates.MOVE);
    }
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
public enum EnityStates
{
    MOVE,
    ATTACK,
    STAGGER
}
