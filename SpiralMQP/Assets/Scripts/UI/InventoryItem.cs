using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class InventoryItem : MonoBehaviour
{
    // each inventory item UI consists: an item image, the quantity of the item and a border image
    [SerializeField] Image itemImage;
    [SerializeField] TMP_Text txt_quantity;
    [SerializeField] Image borderImage;
    private bool empty = true; // a boolean indicator for some of the actions

    // create event action delegates to handle different events
    public event Action<InventoryItem> OnItemClicked, OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag, OnRightMouseBtnClick;


    private void Awake() 
    {
        ResetData();
        Deselect();
    }

    public void ResetData()
    {
        this.itemImage.gameObject.SetActive(false); // hide item image
        empty = true;
    }

    public void Deselect()
    {
        borderImage.enabled = false; // disable border when deselect
    }

    public void SetData(Sprite sprite, int quantity)
    {

    }
    
}
