using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPage : MonoBehaviour
{
    [Header("Major Inventory Components")]
    [SerializeField] InventoryItem item;
    [SerializeField] RectTransform contentPanel;
    [SerializeField] InventoryDescription itemDescription;
    [SerializeField] MouseFollower mouseFollower;

    private int currentDraggingItemIndex = -1;
    public event Action<int> OnDescriptionRequested, OnItemActionRequested, OnStartDragging;
    public event Action<int, int> OnSwapItems;


    List<InventoryItem> listOfItems = new List<InventoryItem>();

    private void Awake()
    {
        itemDescription.ResetDescription();
    }

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

    public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
    {
        if (listOfItems.Count > itemIndex)
        {
            listOfItems[itemIndex].SetData(itemImage, itemQuantity);
        }
    }

    #region All eventhandler fucntions
    private void HandleShowItemActions(InventoryItem item)
    {

    }

    private void HandleSwap(InventoryItem item)
    {
        int index = listOfItems.IndexOf(item);
        if (index == -1) return;

        OnSwapItems?.Invoke(currentDraggingItemIndex, index);
    }

    private void ResetDraggingItem()
    {
        mouseFollower.Toggle(false);
        currentDraggingItemIndex = -1;
    }

    private void HandleEndDrag(InventoryItem item)
    {
        ResetDraggingItem();
    }

    private void HandleBeginDrag(InventoryItem item)
    {
        int index = listOfItems.IndexOf(item);
        if (index == -1) return;
        currentDraggingItemIndex = index;
        HandleItemSelection(item);
        OnStartDragging?.Invoke(index);
    }

    public void CreateDraggedItem(Sprite sprite, int quantity)
    {
        mouseFollower.Toggle(true);
        mouseFollower.SetData(sprite, quantity);
    }

    private void HandleItemSelection(InventoryItem item)
    {
        int index = listOfItems.IndexOf(item);
        if (index == -1) return;
        OnDescriptionRequested?.Invoke(index);
    }
    #endregion
    // show UI
    public void Show()
    {
        gameObject.SetActive(true);
        ResetSelection();
    }

    private void ResetSelection()
    {
        itemDescription.ResetDescription();
        DeselectAllItems();
    }

    private void DeselectAllItems()
    {
        foreach (InventoryItem item in listOfItems)
        {
            item.Deselect();
        }
    }

    // hide UI
    public void Hide()
    {
        ResetDraggingItem();
        gameObject.SetActive(false);
    }
}
