using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float SelfDestroyTime = 10.0f;

    [Range(1.0f, 20.0f)]
    public float Speed = 1.0f;
    
    public Vector3 Direction;

    public float ExplodeRange = 0.5f;

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
        transform.position += Direction * Time.deltaTime * Speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Projectile hit");
        collision.collider.gameObject.GetComponent<OnHit>().Hit();
        Debug.Log(collision.collider.gameObject);
        Destroy(gameObject);
    }
}
