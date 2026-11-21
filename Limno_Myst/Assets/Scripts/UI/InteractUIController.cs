using UnityEngine;
using TMPro;

public class InteractUIController : MonoBehaviour
{
    [SerializeField] private GameObject containerObject;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private TMP_Text promptText;

    // Update is called once per frame
    private void Update()
    {

        // Hide the prompt while dialogue is open
        if (DialogueManager.IsPlaying)
        {
            containerObject.SetActive(false);
            return;
        }

        IInteractable target = playerInteract.GetInteractableObject();
        if (target != null && target.CanInteract)
        {
            promptText.text = "[E] " + target.InteractUIText;
            containerObject.SetActive(true);
        }
        else
        {
            containerObject.SetActive(false);
        }
    }
}
