using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform AttackPosition;
    public float AttackRadius;
    public LayerMask EnemyLayer;

    private Animator cameraAnim;

    private float timeBetweenAttacks;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void Start()
    {
        cameraAnim = Camera.main.GetComponent<Animator>();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && timeBetweenAttacks <= 0)
        {
            GetComponent<PlayerController>().HandleAttackAnimation();

            timeBetweenAttacks = GetComponent<Player>().AttackSpeed;

            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(AttackPosition.position, AttackRadius, EnemyLayer);

            foreach (Collider2D enemyCol in enemiesToDamage)
            {
                Enemy enemy = enemyCol.GetComponent<Enemy>();

                if (enemy.InvTime <= 0)
                {
                    //TODO: % Reduction = (Armor / ([85 * Enemy_Level] + Armor + 400)) * 100
                    // float percent = (float)enemy.GetCharacteristic((int)CharacteristicName.Armour).AdjustedBaseValue /
                    //                 ((85 * enemy.Level) +
                    //                 enemy.GetCharacteristic((int)CharacteristicName.Armour).AdjustedBaseValue + 400);
                    // percent = percent * 100;
                    // dmg = Mathf.RoundToInt(dmg * percent);

                    enemy.TakeDamage(GetComponent<Player>(), enemy);
                }
            }
        }
        else
        {
            timeBetweenAttacks -= Time.deltaTime;
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackPosition.position, AttackRadius);
    }
}
