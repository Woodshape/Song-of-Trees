using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public float AttackSpeed;

    private bool isDamaged;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    protected override void Awake()
    {
        base.Awake();

        GetVital((int)VitalName.Life).AddModifierValue(new ModifyingValue(100));
        GetVital((int)VitalName.Life).CurrentValue = GetVital((int)VitalName.Life).AdjustedBaseValue;

        // GetSkill((int)SkillName.Melee_Defence).AddModifierValue(new ModifyingValue(1));

        StatUpdate();
        // OutputValues();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (InvTime <= 0)
        {
            if (other.CompareTag("Enemy") && !isDamaged)
            {
                if (!other.GetComponent<Enemy>().IsAlive)
                    return;

                // //  TODO: Do _something_ even if we miss or deal no damage
                if (TakeDamage(other.GetComponent<Enemy>(), this) <= 0)
                    return;

                isDamaged = true;
            }
        }

        // else if (other.CompareTag("Interactable"))
        // {
        //     GetComponent<PlayerController>().isAtLadder = true;
        // }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void OnTriggerStay2D(Collider2D other)
    {
        if (InvTime <= 0 && !isDamaged)
        {
            if (other.CompareTag("Enemy"))
            {
                if (!other.GetComponent<Enemy>().IsAlive)
                    return;

                // //  TODO: Do _something_ even if we miss or deal no damage
                if (TakeDamage(other.GetComponent<Enemy>(), this) <= 0)
                    return;

                isDamaged = true;
            }
        }

        if (other.CompareTag("Interactable"))
        {
            GetComponent<PlayerController>().isAtLadder = true;
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            isDamaged = false;
        }

        if (other.CompareTag("Interactable"))
        {
            GetComponent<PlayerController>().isAtLadder = false;
            GetComponent<PlayerController>().isOnLadder = false;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
}
