using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{

    // Item Data
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;
    public string itemDescription;

    // Defines the size of the slot
    [SerializeField] private int maxNumberOfItems;

    // Item Slot
    [SerializeField] private TMP_Text quantityText;

    [SerializeField] private Image itemImage;

    // Item Description Slot
    public Image itemDescriptionImage; // For Item sprite Var
    public TMP_Text ItemDescriptionNameText; // For Item Name Var
    public TMP_Text ItemDescriptionText; // For itemDescription Var

    public GameObject selectedShader;
    public bool thisItemSelected;

    private InventoryManager inventoryManager;

    public void Start()
    {
        inventoryManager = GameObject.Find("Canvas").GetComponent<InventoryManager>();
    }


   public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
   {
    // Check to see if the slot is already full
    if (isFull) {
        return quantity;
    }

    // Update Name
    this.itemName = itemName;

    // Update Image
    this.itemSprite = itemSprite;
    itemImage.sprite = itemSprite;

    // Update Description
    this.itemDescription = itemDescription;

    // Update Quantity
    this.quantity += quantity;

    if (this.quantity >= maxNumberOfItems) {
        quantityText.text = maxNumberOfItems.ToString();
        quantityText.enabled = true;
        isFull = true;
    
        // Return the Leftovers
        int extraItems = this.quantity - maxNumberOfItems;
        this.quantity = maxNumberOfItems;
        return extraItems;
    }

    // Update Quantity Text
    quantityText.text = this.quantity.ToString();
    quantityText.enabled = true;
    return 0;
    } 

   public void OnPointerClick(PointerEventData eventData) 
   {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
   }

   public void OnLeftClick()
   {
        if (thisItemSelected && quantity > 0)
        {
            if (!inventoryManager.IsConsumable(itemName))
            {
                return;
            }

            inventoryManager.UseItem(itemName);
            this.quantity -= 1;
            quantityText.text = this.quantity.ToString();
            if (this.quantity <= 0)
            {
                EmptySlot();
            }
        } 
        else
        {

            inventoryManager.DeselectAllSlots();
            selectedShader.SetActive(true);
            thisItemSelected = true;
            ItemDescriptionNameText.text = itemName;
            ItemDescriptionText.text = itemDescription;
            itemDescriptionImage.sprite = itemSprite;
        }
   }

   private void EmptySlot()
   {
        // Turns off the text and Image for this item slot
        quantityText.enabled = false;
        itemImage.sprite = null;

        // Reset slot's data
        itemName = "";
        itemDescription = "";
        itemSprite = null;
        quantity = 0;
        isFull = false;
        thisItemSelected = false;
        selectedShader.SetActive(false);

        // Clear the data
        ItemDescriptionNameText.text = "";
        ItemDescriptionText.text = "";
        itemDescriptionImage.sprite = null;

   }

   public void OnRightClick()
   {

        // Do nothing if slot empty or item has no drop prefab
        if (quantity <= 0 || inventoryManager.GetDropPrefab(itemName) == null)
        {
            return;
        }

        // Get this item's 3D prefab
        GameObject pref = inventoryManager.GetDropPrefab(itemName);

        // Spawn it near player
        Transform player = GameObject.FindWithTag("Player").transform;
        Vector3 spawn = player.position + player.forward * 1.5f + Vector3.down * 0.9f;
        Instantiate(pref, spawn, Quaternion.identity);

        // Play the drop sound
        inventoryManager.PlayDropSound(spawn);

        // Subtract the item
        this.quantity -= 1;
        quantityText.text = this.quantity.ToString();
        if (this.quantity <= 0)
        {
            EmptySlot();
        }
   }
}
