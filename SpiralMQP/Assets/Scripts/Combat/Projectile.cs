using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float SelfDestroyTime = 5.0f;

    public float Speed = 4.5f;
    
    public Vector3 Direction;

    public float ExplodeRange = 0.5f;

    public LayerMask EnemyLayers;

    void Start(){
        Destroy(gameObject, SelfDestroyTime);
    }

    void Update(){
        transform.position += Direction * Time.deltaTime * Speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {    
        Debug.Log("Projectile hit");
        collision.collider.gameObject.GetComponent<OnHit>().Hit();
        //Destroy this gameobject
        Destroy(gameObject);
    }
}
