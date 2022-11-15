using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
   private Canvas canvas;
   private InventoryItem item;

   private void Awake() 
   {
        canvas = transform.root.GetComponent<Canvas>();
        item = GetComponentInChildren<InventoryItem>();
   }

   public void SetData(Sprite sprite, int quantity)
   {
        item.SetData(sprite, quantity);
   }

   private void Update() 
   {
        Vector2 position;
        
        // using Unity helper function to transform a screen point to a position in the local space of 
        // a RectTransform that is on the plane of its rectangle
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform, 
            Input.mousePosition, 
            canvas.worldCamera, 
            out position); // assign the output value to position
        
        // set the position of current mouse to the same position of the mouse position
        transform.position = canvas.transform.TransformPoint(position);
   }

   public void Toggle(bool val)
   {
        Debug.Log($"Item toggled: {val}");
        gameObject.SetActive(val);
   }
}
