using UnityEngine;

public interface IInteractable
{
    string InteractUIText {get;}
    bool CanInteract => true;
    void Interact(PlayerInteract playerInteract);
}
