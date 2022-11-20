using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class ItemSO : ScriptableObject
    {

        [field: SerializeField] public bool IsStackable { get; set; } // if this item can have multiple duplicates in one slot
        public int ID => GetInstanceID();

        [field: SerializeField] public int MaxStackSize { get; set; } = 1; // max value for the duplicates in one slot
        [field: SerializeField] public string ItemName { get; set; } // item name
        [field: SerializeField][field: TextArea] public string Description { get; set; } // item description
        [field: SerializeField] public Sprite ItemImage { get; set; } // item sprite

    }
}