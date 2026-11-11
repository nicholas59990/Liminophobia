using UnityEngine;

public class Item : MonoBehaviour
{

    [SerializeField] private string itemName;
    [SerializeField] private int quantity;
    [SerializeField] Sprite sprite;

    private InventoyManager inventoyManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoyManager = GameObject.Find("Canvas").GetComponent<InventoyManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inventoyManager.AddItem(itemName, quantity, sprite);
            Destroy(gameObject);
        }
    }
}
