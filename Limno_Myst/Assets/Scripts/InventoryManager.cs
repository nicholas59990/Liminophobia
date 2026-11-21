using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{

    public GameObject InventoryMenu;
    private bool menuAct; // Track menu on/off
    public ItemSlot[] itemSlot;
    public ItemSO[] itemSOs;

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

    // Looks up the dropped prefab for an item by its name
    public GameObject GetDropPrefab(string itemName)
    {
        // Go through every ItemSO we know about
        for (int i = 0; i < itemSOs.Length; i++)
        {
            // if the name matches, hand back the item's dropped prefab
            if (itemSOs[i].itemName == itemName)
            {
                return itemSOs[i].dropPrefab;
            }
        }
        // If no match found
        return null;
    }

    // Returns true if this item can be used by clicking it
    public bool IsConsumable(string itemName)
    {
        for (int i = 0; i < itemSOs.Length; i++)
        {
            if (itemSOs[i].itemName == itemName)
            {
                return itemSOs[i].consumable;
            }
        }
        // If no ItemSO found (Not Usable)
        return false;
    }

    // Returns true if any inventory slot holds that specific item
    public bool HasItem(string itemName)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].itemName == itemName && itemSlot[i].quantity > 0)
            {
                return true;
            }
        }

        return false;
    }
}
