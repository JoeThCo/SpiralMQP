using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float SelfDestroyTime = 10.0f;

    public float Speed = 1.0f;
    
    public Vector3 Direction;

    public float ExplodeRange = 0.5f;

    public LayerMask EnemyLayers;

    public LayerMask PlayerLayer;

    void Start(){
        Destroy(gameObject, SelfDestroyTime);
        Debug.Log(Direction);
    }

    void Update(){
        transform.position += Direction * Time.deltaTime * Speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {    
        Debug.Log("Projectile hit");
        if(collision.collider.gameObject.layer == EnemyLayers){
            collision.collider.gameObject.GetComponent<OnHit>().Hit();
        }
        if(collision.collider.gameObject.layer != PlayerLayer)
        Destroy(gameObject);
    }
}
