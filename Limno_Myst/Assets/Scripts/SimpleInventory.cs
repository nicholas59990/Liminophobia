using UnityEngine;
using System.Collections.Generic;

public class SimpleInventory : MonoBehaviour
{

    // The names of everything the player has pickeed up
    private static List<string> items = new List<string>();

    // Clears the list every time the game starts
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)] private static void ResetInventory()
    {
        items.Clear();
    }

    // Call this when the player picks something up
    public static void Add(string itemName)
    {
        if (!items.Contains(itemName))
        {
            items.Add(itemName);
        }
    }

    // Ask if the player has an item
    public static bool Has(string itemName)
    {
        return items.Contains(itemName);
    }
}
