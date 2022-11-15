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

    [Header("Test")]
    public Sprite _image, _image2;
    public int _quantity;
    public string _title, _description;

    private int currentDraggingItemIndex = -1;


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

    #region All eventhandler fucntions
    private void HandleShowItemActions(InventoryItem item)
    {

    }

    private void HandleSwap(InventoryItem item)
    {
        int index = listOfItems.IndexOf(item);
        if (index == -1)
        {
            mouseFollower.Toggle(false);
            currentDraggingItemIndex = -1;
            return;
        }

        listOfItems[currentDraggingItemIndex].SetData(index == 0 ? _image : _image2, index == 0 ? _quantity : _quantity + 1);
        listOfItems[index].SetData(currentDraggingItemIndex == 0 ? _image : _image2, currentDraggingItemIndex == 0 ? _quantity : _quantity + 1);
        mouseFollower.Toggle(false);
        currentDraggingItemIndex = -1;
    }

    private void HandleEndDrag(InventoryItem item)
    {
        mouseFollower.Toggle(false);
    }

    private void HandleBeginDrag(InventoryItem item)
    {
        int index = listOfItems.IndexOf(item);
        if (index == -1) return;
        currentDraggingItemIndex = index;

        mouseFollower.Toggle(true);
        mouseFollower.SetData(index == 0 ? _image : _image2, index == 0 ? _quantity : _quantity + 1);
    }

    private void HandleItemSelection(InventoryItem item)
    {
        // for test
        listOfItems[0].Select();
        itemDescription.SetDescription(_image, _title, _description);
        Debug.Log("pass");
    }
    #endregion
    // show UI
    public void Show()
    {
        gameObject.SetActive(true);
        itemDescription.ResetDescription();

        listOfItems[0].SetData(_image, _quantity); // for test
        listOfItems[1].SetData(_image2, _quantity + 1);
    }

    // hide UI
    public void Hide()
    {
        mouseFollower.Toggle(false);
        gameObject.SetActive(false);
    }
}
