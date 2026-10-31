using UnityEngine;

public interface IInteractable
{
    string InteractUIText {get;}
    void Interact(PlayerInteract playerInteract);
}
