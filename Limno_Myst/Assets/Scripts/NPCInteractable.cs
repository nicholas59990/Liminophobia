using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable
{

    // The compiled Ink Story for this NPC
    [SerializeField] private TextAsset inkJSON;

    public string InteractUIText => "TALK"; //make it show who you are talking with later

    public void Interact(PlayerInteract playerInteract){
        // Debug.Log("Interacted!");

        DialogueManager.Instance.StartDialogue(inkJSON);
    }
}