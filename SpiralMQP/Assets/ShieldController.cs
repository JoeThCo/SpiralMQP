using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public LayerMask CollisionLayers;
    public int Health;
    private int MaxHealth;

    private void Awake()
    {
        MaxHealth = Health;
    }

    void OnSheildHit(GameObject collisionObj)
    {
        Health--;
        Destroy(collisionObj);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.IsInLayerMask(collision.gameObject, CollisionLayers))
        {
            OnSheildHit(collision.gameObject);
        }
    }
}