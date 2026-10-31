using UnityEngine;

public class HideInteractable : MonoBehaviour
{
    [SerializeField] private Transform hidingSpot;
    [SerializeField] private Transform exitSpot;
    [SerializeField] private Animator animator;

    public Transform HidingSpot => hidingSpot;
    public Transform ExitSpot => exitSpot;
    public string HidingText => IsHiding ? "EXIT" : "HIDE";
    public bool IsHiding {get; private set;}

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
        if(animator != null){
            animator.SetBool("IsHiding", value);
        }
    }
}
