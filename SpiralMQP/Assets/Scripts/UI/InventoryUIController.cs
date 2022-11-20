using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] InventoryPage inventoryPage;
    [SerializeField] InventorySO inventoryData;

    // initialize the inventory at start
    private void Start()
    {
        PrepareUI();
        //inventoryData.Initialize();
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
        
    }

    private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
    {
        
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
        inventoryPage.UpdateDescription(itemIndex, item.ItemImage, item.name, item.Description);
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

    }
}
