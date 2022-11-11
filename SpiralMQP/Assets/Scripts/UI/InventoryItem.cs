using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

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
        itemImage.gameObject.SetActive(false); // hide item image
        empty = true;
    }

    public void Deselect()
    {
        borderImage.enabled = false; // disable border when deselect
    }

    public void SetData(Sprite sprite, int quantity)
    {
        itemImage.gameObject.SetActive(true);
        itemImage.sprite = sprite;
        txt_quantity.text = quantity + "";
        empty = false;
    }

    public void Select()
    {
        borderImage.enabled = true;
    }
    
    public void OnBeginDrag()
    {
        if(empty) return; // if the item is empty, dragging does nothing
        OnItemBeginDrag?.Invoke(this);
    }

    public void OnDrop()
    {
        OnItemDroppedOn?.Invoke(this);
    }

    public void OnEndDrag()
    {
        OnItemEndDrag?.Invoke(this);
    }

    public void OnPointerClick(BaseEventData data)
    {
        if (empty) return; // if the item is empty, clicking does nothing
        
        // tracking mouse click event data
        PointerEventData pointerData = (PointerEventData)data;
        if (pointerData.button == PointerEventData.InputButton.Right) 
        {
            OnRightMouseBtnClick?.Invoke(this);
        }
        else OnItemClicked?.Invoke(this);
    }

}
