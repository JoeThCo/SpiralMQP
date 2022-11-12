using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public KeyCode MeleeKey = KeyCode.E;

    public int AttackCoolDown = 2;
    private float AttackCoolDownCounter;

    public float AttackRange = 0.5f;

    public LayerMask EnemyLayers;

    void Start()
    {
        AttackCoolDownCounter = AttackCoolDown;
    }

    void Update()
    {
        if (AttackCoolDown < AttackCoolDownCounter)
        {
            AttackCoolDownCounter += Time.deltaTime;
        }
        else
        {
            if (Input.GetKeyDown(MeleeKey))
            {
                Attack();
                AttackCoolDownCounter = 0;
            }
        }
    }

    void Attack()
    {
        Debug.Log("Attack");
        Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(transform.position, AttackRange, EnemyLayers);
        foreach (Collider2D enemy in hitEnemys)
        {
            Debug.Log("Hit");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
}
