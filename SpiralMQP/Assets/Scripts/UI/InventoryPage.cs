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
    public Sprite _image;
    public int _quantity;
    public string _title, _description;


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
    private void HandleShowItemActions(InventoryItem obj)
    {
        
    }

    private void HandleSwap(InventoryItem obj)
    {
        
    }

    private void HandleEndDrag(InventoryItem obj)
    {
        mouseFollower.Toggle(false);
    }

    private void HandleBeginDrag(InventoryItem obj)
    {
        mouseFollower.Toggle(true);
        mouseFollower.SetData(_image, _quantity);
    }

    private void HandleItemSelection(InventoryItem obj)
    {
        // for test
        listOfItems[0].Select();
        itemDescription.SetDescription(_image,_title,_description);
        Debug.Log("pass");
    }
    #endregion
    // show UI
    public void Show()
    {
        gameObject.SetActive(true);
        itemDescription.ResetDescription();

        listOfItems[0].SetData(_image,_quantity); // for test
    }

    // hide UI
    public void Hide()
    {
        mouseFollower.Toggle(false);
        gameObject.SetActive(false);
    }
}
