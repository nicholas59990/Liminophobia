using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private float interactRange = 2f;
    private PlayerMovement movement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if(DialogueManager.IsPlaying){
            return;
        }
        if(Input.GetKeyDown(KeyCode.E)){
            IInteractable target = GetInteractableObject();
            if(target != null){
                target.Interact(this);
            }
            /*
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            foreach(Collider collider in colliderArray){
                if(collider.TryGetComponent(out NPCInteractable npcInteractable)){
                    npcInteractable.Interact();
                }
            }
            */
        }
    }

    private void Awake(){
        movement = GetComponent<PlayerMovement>();
    }

    public IInteractable GetInteractableObject(){
        //List<NPCInteractable> npcInteractableList = new List<NPCInteractable>();
        if(movement.IsTransitioning){
            return null;
        }
        if(movement.IsHidden && movement.CurrentSpot != null){
            return movement.CurrentSpot;
        }
        IInteractable closest = null;
        float closestDistance = float.MaxValue;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach(Collider collider in colliderArray){
            /*
            if(collider.TryGetComponent(out NPCInteractable npcInteractable)){
                npcInteractableList.Add(npcInteractable);
            }
            */
            IInteractable interactable = collider.GetComponentInParent<IInteractable>();
            if(interactable == null){
                continue;
            }
            float distance = Vector3.Distance(transform.position, ((MonoBehaviour)interactable).transform.position);
            if(distance < closestDistance){
                closestDistance = distance;
                closest = interactable;
            }
        }
        /*
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
        */
        return closest;        
    }
}
