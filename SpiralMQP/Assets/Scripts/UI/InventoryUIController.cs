using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] InventoryPage inventoryPage;
    [SerializeField] int capacity;

    // initialize the inventory at start
    private void Start() 
    {
        inventoryPage?.InitializedInventoryUI(capacity); // check if null
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
            }
            else inventoryPage.Hide();
        }

    }
}
