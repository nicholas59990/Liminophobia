using UnityEngine;

public class HideInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform hidingSpot;
    [SerializeField] private Transform exitSpot;
    [SerializeField] private Animator animator;

    public Transform HidingSpot => hidingSpot;
    public Transform ExitSpot => exitSpot;
    public bool IsHiding {get; private set;}
    public string InteractUIText => IsHiding ? "Exit" : "Hide";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake(){
        if(animator == null){
            animator = GetComponent<Animator>();
        }
    }

    public void Interact(PlayerInteract playerInteract){
        PlayerMovement movement = playerInteract.GetComponent<PlayerMovement>();
        if(movement.IsTransitioning){
            return;
        }
        if(IsHiding){
            movement.StopHiding();
        }else{
            movement.StartHiding(this);
        }
    }

    public void SetIsHiding(bool value){
        IsHiding = value;
    }

    public void SetDoorOpen(bool open){
        if(animator != null){
            animator.SetBool("DoorOpen", open);
        }
    }
}
