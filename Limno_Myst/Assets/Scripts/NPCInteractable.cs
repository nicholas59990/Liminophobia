using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable
{

    // The compiled Ink Story for this NPC
    [SerializeField] private TextAsset inkJSON;

    // What the NPC says after the player has the key
    [SerializeField] private TextAsset inkJSONAfterKey;

    // The item that switches the dialogue (must match the key's item name)
    [SerializeField] private string itemNeeded = "Key";

    // Where the camera goes while talking
    [SerializeField] private Transform dialogueCameraPoint;

    public string InteractUIText => "TALK"; //make it show who you are talking with later

    public void Interact(PlayerInteract playerInteract){

        // Ignore it if a dialogue is already open
        if (DialogueManager.IsPlaying)
        {
            return;
        }

        // Move the camera in front of the NPC
        if (dialogueCameraPoint != null)
        {
            Camera.main.transform.position = dialogueCameraPoint.position;
            Camera.main.transform.rotation = dialogueCameraPoint.rotation;
        }

        // Pick which dialogue to play
        TextAsset story = inkJSON;

        if (inkJSONAfterKey != null && SimpleInventory.Has(itemNeeded))
        {
            story = inkJSONAfterKey;
        }

        DialogueManager.Instance.StartDialogue(story);
    }
}