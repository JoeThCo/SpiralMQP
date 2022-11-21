using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public int ShieldHealth;
    private int shieldMaxHealth;

    public LayerMask CollisionLayers;

    private void Awake()
    {
        shieldMaxHealth = ShieldHealth;
    }

    void OnSheildHit(GameObject collisionObj)
    {
        ShieldHealth--;
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