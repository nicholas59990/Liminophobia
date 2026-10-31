using UnityEngine;
using TMPro;

public class InteractUIController : MonoBehaviour
{
    [SerializeField] private GameObject containerObject;
    [SerializeField] private PlayerInteract playerInteract;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {

        // Hide the prompt while dialogue is open
        if (DialogueManager.IsPlaying)
        {
            containerObject.SetActive(false);
            return;
        }

        if(playerInteract.GetInteractableObject() != null){
            Show(playerInteract.GetInteractableObject());
        }else{
            Hide();
        }
    }

    private void Show(NPCInteractable npcInteractable){
        containerObject.SetActive(true);
    }

    private void Hide(){
        containerObject.SetActive(false);
    }
}
