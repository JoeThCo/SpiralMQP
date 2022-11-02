using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour {
    public int AttackCoolDown = 2;
    public int AttackCountDown;
    public float AttackRange = 0.5f;
    public Transform AttackPoint;
    public LayerMask EnemyLayers;

    void Start() {
        AttackCountDown = AttackCoolDown;
    }
    void Update() {
        AttackCountDown--;
        if(Input.GetKeyDown(KeyCode.Q)){
            if(AttackCountDown <= 0){
                Attack();
                AttackCountDown = AttackCoolDown;
            }
        }
    }

    void Attack() {
        Debug.Log("attack");
        Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, EnemyLayers);
        foreach(Collider2D enemy in hitEnemys){
            Debug.Log("Hit");
        }
    }

    void OnDrawGizmosSelected(){
        if(AttackPoint == null)
        return;
        Gizmos.DrawWireSphere(AttackPoint.position,AttackRange);
    }
}
