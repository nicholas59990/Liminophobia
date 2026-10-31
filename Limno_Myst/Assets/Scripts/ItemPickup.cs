using UnityEngine;

public class ItemPickup : MonoBehaviour
{

    // The item's name
    [SerializeField] private string itemName = "Key";
    [SerializeField] private bool completesObjective = false;
    [SerializeField] private string nextObjective = "";

    // The pickup sound
    [SerializeField] private AudioClip pickupSound;

    // Runs when something enters the item's trigger
    private void OnTriggerEnter(Collider other)
    {

        // Only react to the player
        if (!other.CompareTag("Player"))
        {
            return;
        }

        // TODO: when the inventory exists, add the item here

        // Play the pickup sound
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }

        Debug.Log("Picked up " + itemName);

        // Finish the current objective if the item is meant to
        if (completesObjective)
        {
            ObjectiveHUD.Instance.CompleteObjective();
        }

        // Show the next objective
        if (nextObjective != "")
        {
            ObjectiveHUD.Instance.SetObjective(nextObjective);
        }

        // Remove the item from the world
        Destroy(gameObject);
    }
}
