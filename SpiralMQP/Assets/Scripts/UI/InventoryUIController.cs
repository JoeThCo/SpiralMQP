using System;
using System.Collections;
using System.Collections.Generic;
using Inventory.Model;
using Inventory.UI;
using UnityEngine;

namespace Inventory
{
    public class InventoryUIController : MonoBehaviour
    {
        [SerializeField] InventoryPage inventoryPage;
        [SerializeField] InventorySO inventoryData;
        [SerializeField] LayerMask ItemLayer;

        public List<InventoryItemStruct> initialItems = new List<InventoryItemStruct>(); // for test purpose 

        // initialize the inventory at start
        private void Start()
        {
            PrepareUI();
            PrepareInventoryData();
        }

        private void PrepareInventoryData()
        {
            inventoryData.Initialize();
            inventoryData.OnInventoryUpdated += UpdateInventoryUI;
            foreach (InventoryItemStruct item in initialItems)
            {
                if (item.isEmpty) continue;
                inventoryData.AddItem(item);
            }
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItemStruct> inventoryState)
        {
            inventoryPage.ResetAllItems();
            foreach (var item in inventoryState)
            {
                inventoryPage.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
            }
        }

        private void PrepareUI()
        {
            inventoryPage?.InitializedInventoryUI(inventoryData.size); // check if null
            inventoryPage.OnDescriptionRequested += HandleDescriptionRequest;
            inventoryPage.OnSwapItems += HandleSwapItems;
            inventoryPage.OnStartDragging += HandleDragging;
            inventoryPage.OnItemActionRequested += HandleItemActionRequest;
        }

        private void HandleItemActionRequest(int itemIndex)
        {

        }

        private void HandleDragging(int itemIndex)
        {
            InventoryItemStruct inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.isEmpty) return;

            inventoryPage.CreateDraggedItem(inventoryItem.item.ItemImage, inventoryItem.quantity);

        }

        private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
        {
            inventoryData.SwapItems(itemIndex_1, itemIndex_2);
        }

        private void HandleDescriptionRequest(int itemIndex)
        {
            InventoryItemStruct inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.isEmpty)
            {
                inventoryPage.ResetSelection();
                return;
            }

            ItemSO item = inventoryItem.item;
            inventoryPage.UpdateDescription(itemIndex, item.ItemImage, item.ItemName, item.Description);
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                // toggle on and off inventory UI accordingly
                if (inventoryPage.isActiveAndEnabled == false)
                {
                    inventoryPage.Show();
                    foreach (var item in inventoryData.GetCurrentInventoryState())
                    {
                        inventoryPage.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
                    }
                }
                else inventoryPage.Hide();
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                Collider2D[] items = Physics2D.OverlapCircleAll(transform.position, 2.0f, ItemLayer);
                foreach (Collider2D item in items)
                {
                    if(item.gameObject.GetComponent<CollectableItemController>()){
                        if(inventoryData.AddItem(item.gameObject.GetComponent<CollectableItemController>().Item,1))
                        {
                            Destroy(item.gameObject);
                        }
                    }
                }
            }

        }
    }
}