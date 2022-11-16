using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float SelfDestroyTime = 10.0f;    
    public Vector3 Velocity;
    public Vector3 Direction;

    public LayerMask EnemyLayers;
    public LayerMask PlayerLayer;
    void Start(){
        Destroy(gameObject, SelfDestroyTime);
        double angle = Mathf.Atan(Direction.y/Direction.x)*180.0/3.1416;
        if(Direction.x>0){
            transform.rotation = Quaternion.Euler(0, 0, 180+(float)angle);
        } else{
            transform.rotation = Quaternion.Euler(0, 0, (float)angle);
        }
    }

    void Update(){
        transform.position += Velocity * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject.transform.parent)
            collision.collider.gameObject.transform.parent.gameObject.GetComponent<OnHit>().Hit();
        Destroy(gameObject);
    }
}
