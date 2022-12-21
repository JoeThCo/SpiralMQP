using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public KeyCode MeleeKey = KeyCode.E;
    [SerializeField] AudioClip meleeAudio;
    [SerializeField] AudioClip enemyHitAudio;

    [Space(10)]

    [SerializeField] Animator playerAnimator;

    [Space(10)]

    [SerializeField] int AttackCoolDown = 2;
    private float AttackCoolDownCounter;

    [Space(10)]

    [SerializeField] float AttackRange = 1.0f;
    [SerializeField] LayerMask EnemyLayers;

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
            }
        }
    }

    void Attack()
    {
        SoundManager.PlayOneShot(meleeAudio, gameObject);
        playerAnimator.SetTrigger("tr_Melee"); // start player melee animation

        Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(transform.position, AttackRange, EnemyLayers);

        foreach (Collider2D enemy in hitEnemys)
        {
            enemy.gameObject.transform.parent.gameObject.GetComponent<OnHit>().Hit();
        }

        AttackCoolDownCounter = 0;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
}
