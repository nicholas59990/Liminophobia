using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerInteract : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] private float interactDotThreshold = 0.5f;
    private float interactRange = 2f;
    private PlayerMovement movement;
    private Camera playerCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (DialogueManager.IsPlaying)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            IInteractable target = GetInteractableObject();
            target?.Interact(this);
        }
    }

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    public IInteractable GetInteractableObject()
    {
        if (movement.IsTransitioning)
        {
            return null;
        }
        if (movement.IsHidden && movement.CurrentSpot != null)
        {
            return movement.CurrentSpot;
        }
        IInteractable closest = null;
        float closestDistance = float.MaxValue;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray)
        {
            IInteractable interactable = collider.GetComponentInParent<IInteractable>();
            if (interactable == null)
            {
                continue;
            }
            if (!interactable.CanInteract)
            {
                continue;
            }
            if (!interactable.IsFacingInteractable(this))
            {
                continue;
            }
            float distance = Vector3.Distance(transform.position, ((MonoBehaviour)interactable).transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = interactable;
            }
        }
        return closest;
    }
}
