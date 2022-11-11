using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPage : MonoBehaviour
{
    [SerializeField] InventoryItem item;

    [SerializeField] RectTransform contentPanel;

    List<InventoryItem> listOfItems = new List<InventoryItem>();

    // initialize inventory item UI with desired size
    public void InitializedInventoryUI(int inventorySize)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            InventoryItem inventoryItem = Instantiate(item, Vector3.zero, Quaternion.identity);
            inventoryItem.transform.SetParent(contentPanel);
            listOfItems.Add(inventoryItem);

            // event subscription
            inventoryItem.OnItemClicked += HandleItemSelection;
            inventoryItem.OnItemBeginDrag += HandleBeginDrag;
            inventoryItem.OnItemEndDrag += HandleEndDrag;
            inventoryItem.OnItemDroppedOn += HandleSwap;
            inventoryItem.OnRightMouseBtnClick += HandleShowItemActions;
        }
    }

    #region All eventhandler fucntions
    private void HandleShowItemActions(InventoryItem obj)
    {
        
    }

    private void HandleSwap(InventoryItem obj)
    {
        
    }

    private void HandleEndDrag(InventoryItem obj)
    {
        
    }

    private void HandleBeginDrag(InventoryItem obj)
    {
        
    }

    private void HandleItemSelection(InventoryItem obj)
    {
        Debug.Log("pass");
    }
    #endregion
    // show UI
    public void Show()
    {
        gameObject.SetActive(true);
    }

    // hide UI
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
