using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)){
            float interactRange = 2f;
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            foreach(Collider collider in colliderArray){
                if(collider.TryGetComponent(out NPCInteractable npcInteractable)){
                    npcInteractable.Interact();
                }
            }
        }
    }

    public NPCInteractable GetInteractableObject(){
        List<NPCInteractable> npcInteractableList = new List<NPCInteractable>();
        float interactRange = 2f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach(Collider collider in colliderArray){
            if(collider.TryGetComponent(out NPCInteractable npcInteractable)){
                npcInteractableList.Add(npcInteractable);
            }
        }
        NPCInteractable closestNPC = null;
        foreach(NPCInteractable npc in npcInteractableList){
            if(closestNPC == null){
                closestNPC = npc;
            }else{
                if(Vector3.Distance(transform.position, npc.transform.position) < Vector3.Distance(transform.position, closestNPC.transform.position)){
                    closestNPC = npc;
                }
            }
        }
        return closestNPC;        
    }
}
