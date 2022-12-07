using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public bool HasContactDamage;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(HasContactDamage && collision.collider.gameObject.name == "Player")
            collision.collider.gameObject.GetComponent<PlayerHit>().Hit();
    }
}