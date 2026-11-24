using UnityEngine;

public class HideInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform hidingSpot;
    [SerializeField] private Transform exitSpot;
    [SerializeField] private Animator animator;

    public Transform HidingSpot => hidingSpot;
    public Transform ExitSpot => exitSpot;
    public bool IsHiding { get; private set; }
    public string InteractUIText => IsHiding ? "Exit" : "Hide";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    public void Interact(PlayerInteract playerInteract)
    {
        PlayerMovement movement = playerInteract.GetComponent<PlayerMovement>();
        if (movement.IsTransitioning)
        {
            return;
        }
        if (IsHiding)
        {
            movement.StopHiding();
        }
        else
        {
            movement.StartHiding(this);
        }
    }

    public void SetIsHiding(bool value)
    {
        IsHiding = value;
    }

    public bool IsFacingInteractable(PlayerInteract playerInteract)
    {
        Vector3 front = exitSpot.position - hidingSpot.position;
        front.y = 0f;
        front.Normalize();
        Vector3 toPlayer = playerInteract.transform.position - hidingSpot.position;
        toPlayer.y = 0f;
        toPlayer.Normalize();
        return Vector3.Dot(front, toPlayer) > 0.5f;
    }
}
