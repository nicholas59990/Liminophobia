using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{

    public GameObject InventoryMenu;
    private bool menuAct; // Track menu on/off
    public ItemSlot[] itemSlot;
    public ItemSO[] itemSOs;

    // Sound to play when player drops a item
    [SerializeField] public AudioClip dropSound;

    // Used to check if player hiding or in a transition
    private PlayerMovement playerMovement;

    [Header("Hotbar")]
    public ItemSlot[] hotbarSlots; 

    void Start()
    {
        // Find the player's movement script in the scene
        playerMovement = FindFirstObjectByType<PlayerMovement>();
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
            // DO not open inventory while dialogue is occuring
            if (DialogueManager.IsPlaying)
            {
                return;
            }

            // Do not open inventory while hiding or during a fade
            if (playerMovement != null && (playerMovement.IsHidden || playerMovement.IsTransitioning))
            {
                return;
            }

            Time.timeScale = 0;
            InventoryMenu.SetActive(true); // activates menu
            menuAct = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Hotbar keys
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UseHotbar(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UseHotbar(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UseHotbar(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            UseHotbar(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            UseHotbar(4);
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

    // Plays the drop sound at the given position
    public void PlayDropSound(Vector3 position)
    {
        if (dropSound != null)
        {
            // AudioClip.PlayClipAtPoint(dropSound, Camera.main.transform.position);
            AudioSource.PlayClipAtPoint(dropSound, position);
        }
    }

    // Called when the player clicks a hotbar slot while an item is selected in the inventory
    public void AssignToHotbar(int hotbarIndex, ItemSlot sourceSlot)
    {
        if (hotbarIndex < 0 || hotbarIndex >= hotbarSlots.Length)
        {
            return;
        }

        if (sourceSlot == null || sourceSlot.quantity <= 0)
        {
            return;
        }

        // remove this item from any other hotbar slots (prevent same item to occupy multiple hotbar slots)
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            if (hotbarSlots[i].itemName == sourceSlot.itemName)
            {
                hotbarSlots[i].EmptySlot();
            }
        }

        ItemSlot hotbar = hotbarSlots[hotbarIndex];

        // Copy the item data from inventory slot into the hotbar slot
        hotbar.itemName = sourceSlot.itemName;
        hotbar.quantity = sourceSlot.quantity;
        hotbar.itemSprite = sourceSlot.itemSprite;
        hotbar.itemDescription = sourceSlot.itemDescription;
        hotbar.isFull = true;

        // Update the visual
        hotbar.itemImage.sprite = sourceSlot.itemSprite;
        hotbar.quantityText.text = sourceSlot.quantity.ToString();
        hotbar.quantityText.enabled = true;
    }

    // Called when pressing keys 1 through 5
    public void UseHotbar(int hotkey)
    {
        if (hotkey < 0 || hotkey >= hotbarSlots.Length)
        {
            return;
        }

        ItemSlot slot = hotbarSlots[hotkey];

        // DO nothing if the slot is empty
        if (slot.quantity <= 0)
        {
            return;
        }

        // Only use if it is a consumable
        if (!IsConsumable(slot.itemName))
        {
            return;
        }

        // Actually use the item (Calls ItemSO.UseItem here)
        UseItem(slot.itemName);

        // Reduce quantity
        slot.quantity--;
        slot.quantityText.text = slot.quantity.ToString();

        // Clear the slot if it reaches zero
        if (slot.quantity <= 0)
        {
            slot.EmptySlot();
        }
    }

    // Removes an item from all hotbar slots
    public void ClearHotbarItem(string itemName)
    {
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            if (hotbarSlots[i].itemName == itemName)
            {
                hotbarSlots[i].EmptySlot();
            }
        }
    }
}
