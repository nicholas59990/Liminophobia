using UnityEngine;

public class InventoyManager : MonoBehaviour
{

    public GameObject InventoryMenu;
    private bool menuAct; // Track menu on/off

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
}
