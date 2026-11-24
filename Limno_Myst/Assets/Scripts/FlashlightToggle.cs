using UnityEngine;

public class FlashlightToggle : MonoBehaviour
{
    [SerializeField] private Light lightObject;
    private bool on = false;

    // Click sound for toggling flashlight on/off
    [SerializeField] private AudioClip toggleSound;

    // Used to check if player hiding or in a transition
    private PlayerMovement playerMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lightObject.enabled = on;

        // Find the player's movement script in the scene
        playerMovement = FindFirstObjectByType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {

        // Ignore clicks while the inventory is open ()
        if (Time.timeScale == 0f)
        {
            return;
        }

        // Ignore clicks while a dialogue is playing
        if (DialogueManager.IsPlaying)
        {
            return;
        }

        // Ignore clicks while the player is hiding
        if (playerMovement != null && playerMovement.IsHidden)
        {
            lightObject.enabled = on;
            return;
        }

        // Left click toggles the flashlight
        if (Input.GetMouseButtonDown(0))
        {
            lightObject.enabled = !lightObject.enabled;

            // Play the sound
            if (toggleSound != null)
            {
                AudioSource.PlayClipAtPoint(toggleSound, transform.position);
            }
        }
    }
}
