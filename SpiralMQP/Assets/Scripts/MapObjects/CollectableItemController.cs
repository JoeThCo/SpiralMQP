using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory.Model;

public class CollectableItemController : MonoBehaviour
{
    [SerializeField] SpriteRenderer Sprite;
    [SerializeField] public ItemSO Item;


    void Start()
    {
        Sprite = GetComponent<SpriteRenderer>();
        if (Sprite)
        {
            Sprite.sortingOrder = (int)(-transform.position.y * 100.0f);
            Sprite.sortingLayerName = "Character";
            Sprite.sprite = Item.ItemImage;
        }
    }
}
