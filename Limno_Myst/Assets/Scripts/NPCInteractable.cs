using UnityEngine;

public class NPCInteractable : MonoBehaviour
{

    // The compiled Ink Story for this NPC
    [SerializeField] private TextAsset inkJSON;

    public void Interact(){
        // Debug.Log("Interacted!");

        DialogueManager.Instance.StartDialogue(inkJSON);
    }
}