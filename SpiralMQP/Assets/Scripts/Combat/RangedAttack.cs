using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    public Vector3 ShootDirection;

    public float ProjectileSpeed = 10.0f;

    public Vector3 ShootVelocity;

    public float AttackCoolDown = 0.3f;

    public bool CanShoot = true;

    public bool OnDash = false;

    public GameObject ProjectilePrefab;

    public Transform LaunchOffset;

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0)&&CanShoot&&!OnDash)
        {
            StartCoroutine(Launch());
        }
    }

    IEnumerator Launch()
    {
        // Get direction
        ShootDirection = Input.mousePosition;
        ShootDirection.z = 0.0f;
        ShootDirection = Camera.main.ScreenToWorldPoint(ShootDirection);
        ShootDirection = ShootDirection-transform.position;
        ShootDirection.z = 0.0f;
        ShootDirection.Normalize();
        ShootVelocity = ShootDirection*ProjectileSpeed;
        // Adding Player Movement component
        Vector2 playerVelocity = gameObject.GetComponent<PlayerMovement>().inputDir*gameObject.GetComponent<PlayerMovement>().MovementSpeed;
        ShootVelocity += new Vector3(playerVelocity.x,playerVelocity.y,0.0f)*0.5f;
        // Launch at player position
        Vector3 pos = transform.position;
        Quaternion rotation = transform.rotation;
        GameObject Projectile = Instantiate(ProjectilePrefab, pos + ShootDirection/2, rotation);
        Projectile.GetComponent<Projectile>().Velocity = ShootVelocity;
        Projectile.GetComponent<Projectile>().Direction = ShootDirection;

        CanShoot = false;
        yield return new WaitForSeconds(AttackCoolDown);
        CanShoot = true;
    }
}
