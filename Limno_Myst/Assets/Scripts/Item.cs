using UnityEngine;

public class Item : MonoBehaviour
{

    [SerializeField] public string itemName;
    [SerializeField] public int quantity;
    [SerializeField] public Sprite sprite;

    [TextArea]
    [SerializeField] public string itemDescription;

    // Sound for pickup (FILL IN ON THE DROPPED PREFAB ONLY)
    // Leave empty on scene key because ItemPickup plays sound
    [SerializeField] private AudioClip pickupSound;

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
                // Play the pickup sound
                if (pickupSound != null)
                {
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                }

                Destroy(gameObject);
            } 
            // else { // If leftovers change the quantity of this item
            //     quantity = leftOverItems;
            // }
        }
    }
}
