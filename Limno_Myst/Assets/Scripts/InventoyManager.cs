using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoyManager : MonoBehaviour
{

    public GameObject InventoryMenu;
    private bool menuAct; // Track menu on/off
    public ItemSlot[] itemSlot;

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
        }

        else if (Input.GetButtonDown("Inventory") && !menuAct)
        {
            Time.timeScale = 0;
            InventoryMenu.SetActive(true); // activates menu
            menuAct = true;
        }
    }

    public void AddItem(string itemName, int quantity, Sprite itemSprite)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].isFull == false)
            {
                itemSlot[i].AddItem(itemName, quantity, itemSprite);
                return;
            }
        }
    }
}
