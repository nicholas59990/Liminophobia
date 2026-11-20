using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{

    public GameObject InventoryMenu;
    private bool menuAct; // Track menu on/off
    public ItemSlot[] itemSlot;
    public ItemSO[] itemSOs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory") && menuAct)
        {
            Time.timeScale = 1;
            InventoryMenu.SetActive(false); // deactivates menu
            menuAct = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        else if (Input.GetButtonDown("Inventory") && !menuAct)
        {
            Time.timeScale = 0;
            InventoryMenu.SetActive(true); // activates menu
            menuAct = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void UseItem(string itemName)
    {

        for (int i = 0; i < itemSOs.Length; i++)
        {
            if (itemSOs[i].itemName == itemName)
            {
                itemSOs[i].UseItem();
                return;
            }
        }
    }

    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            // If itemSlot is not full and the slot has the same item as this one or if the slot is completely empty
            if (itemSlot[i].isFull == false && (itemSlot[i].itemName == itemName || itemSlot[i].quantity == 0))
            {
                int leftOverItems = itemSlot[i].AddItem(itemName, quantity, itemSprite, itemDescription);
                if (leftOverItems > 0) {
                    leftOverItems = AddItem(itemName, leftOverItems, itemSprite, itemDescription);
                }

                return leftOverItems;
            }
        }
        return quantity; // inventory is full
    }

    public void DeselectAllSlots()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].selectedShader.SetActive(false);
            itemSlot[i].thisItemSelected = false;
        }
    }
}
