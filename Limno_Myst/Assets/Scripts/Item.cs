using UnityEngine;

public class Item : MonoBehaviour
{

    [SerializeField] private string itemName;
    [SerializeField] private int quantity;
    [SerializeField] Sprite sprite;

    [TextArea]
    [SerializeField] string itemDescription;

    private InventoryManager inventoryManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryManager = GameObject.Find("Canvas").GetComponent<InventoryManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int leftOverItems = inventoryManager.AddItem(itemName, quantity, sprite, itemDescription);
            if (leftOverItems <= 0) {
                Destroy(gameObject);
            } else { // If leftovers change the quantity of this item
                quantity = leftOverItems;
            }
        }
    }
}
