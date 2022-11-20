using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class InventorySO : ScriptableObject
    {
        [SerializeField] List<InventoryItemStruct> inventoryItems;
        [field: SerializeField] public int size { get; set; } = 10;

        public void Initialize()
        {
            inventoryItems = new List<InventoryItemStruct>();
            for (int i = 0; i < size; i++)
            {
                inventoryItems.Add(InventoryItemStruct.GetEmptyItem());
            }
        }

        public void AddItem(ItemSO item, int quantity)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].isEmpty)
                {
                    inventoryItems[i] = new InventoryItemStruct
                    {
                        item = item,
                        quantity = quantity
                    };
                }
            }
        }

        public Dictionary<int, InventoryItemStruct> GetCurrentInventoryState()
        {
            Dictionary<int, InventoryItemStruct> returnValue = new Dictionary<int, InventoryItemStruct>();
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].isEmpty) continue;
                returnValue[i] = inventoryItems[i];
            }
            return returnValue;
        }

        public InventoryItemStruct GetItemAt(int itemIndex)
        {
            return inventoryItems[itemIndex];
        }
    }

    [Serializable]
    public struct InventoryItemStruct
    {
        public int quantity;
        public ItemSO item;
        public bool isEmpty => item == null;

        public InventoryItemStruct ChangeQuantity(int newQuantity)
        {
            return new InventoryItemStruct
            {
                item = item,
                quantity = newQuantity
            };
        }

        public static InventoryItemStruct GetEmptyItem() => new InventoryItemStruct { item = null, quantity = 0 };

    }
}