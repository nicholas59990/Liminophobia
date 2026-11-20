using UnityEngine;

public interface IInteractable
{
    string InteractUIText { get; }
    bool CanInteract => true;
    bool IsFacingInteractable(PlayerInteract playerInteract) => true;
    void Interact(PlayerInteract playerInteract);
}