using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private SpriteRenderer Sprite;

    void Start(){
        Sprite = GetComponent<SpriteRenderer>();
        UpdateSortingOrder();
    }

    void UpdateSortingOrder(){
        if (Sprite)
        {
            Sprite.sortingOrder = (int)(-transform.position.y*100.0f);
        }
    }
}