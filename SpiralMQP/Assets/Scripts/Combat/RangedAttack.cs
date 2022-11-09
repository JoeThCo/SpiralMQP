using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    public Vector3 shootDirection;

    public float AttackCoolDown = 1.0f;

    public float AttackCoolDownCounter;

    public GameObject ProjectilePrefab;

    public Transform LaunchOffset;

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
            if (Input.GetMouseButtonDown(0))
            {
                Launch();
                AttackCoolDownCounter = 0;
            }
        }
    }

    void Launch()
    {
        shootDirection = Input.mousePosition;
        shootDirection.z = 0.0f;
        shootDirection = Camera.main.ScreenToWorldPoint(shootDirection);
        shootDirection = shootDirection-transform.position;
        shootDirection.z = 0.0f;
        shootDirection.Normalize();
        Vector3 pos = transform.position;
        Quaternion rotation = transform.rotation;
        GameObject Projectile = Instantiate(ProjectilePrefab, pos, rotation);
        Projectile.GetComponent<Projectile>().Direction = shootDirection;
        Physics2D.IgnoreCollision(Projectile.GetComponent<Collider2D>(),GetComponent<Collider2D>());
    }
}
