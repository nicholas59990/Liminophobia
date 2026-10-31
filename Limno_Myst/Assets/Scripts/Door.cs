using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{

    [Header("Lock")]
    // Tick this if the door needs a key
    [SerializeField] private bool isLocked= true;

    // The item name the door needs
    [SerializeField] private string keyNeeded = "Key";

    [Header("Objectives")]
    // Tick this if opening the door finishes the current objective
    [SerializeField] private bool completesObjective = false;

    // TODO Add the next objective which is repair the exit door 
    // The next objective to show
    [SerializeField] private string nextObjective = "";

    [Header("Opening")]
    // The part that swings open (the hinge)
    [SerializeField] private Transform hinge;

    // How far it swings
    [SerializeField] private float openAngle = 90f;

    // How fast it swings
    [SerializeField] private float openSpeed = 60f;

    [Header("Sounds")]
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip lockedSound;

    [Header("Dialogue")]
    [SerializeField] private TextAsset lockedDialogue;

    // True while the player is standing near the door
    private bool playerNear;

    // True once the door has been opened
    private bool isOpen;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    public string InteractUIText => "Open Door";
    public bool CanInteract => !isOpen;
    
    public void Interact(PlayerInteract playerInteract){
        TryOpen();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Remember the closed rotation and work out the open one
        closedRotation = hinge.localRotation;
        openRotation = closedRotation * Quaternion.Euler(0f, openAngle, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        // Swing the door toward open or closed
        Quaternion target = isOpen ? openRotation : closedRotation;
        hinge.localRotation = Quaternion.RotateTowards(hinge.localRotation, target, openSpeed * Time.deltaTime);
    }

    private void TryOpen()
    {
        // TODO add sound
        // Locked and no key (play the locked sound and stop)
        
            if (isLocked && !SimpleInventory.Has(keyNeeded))
            {
                Debug.Log("The door is locked");

                if (lockedSound != null)
                {
                    AudioSource.PlayClipAtPoint(lockedSound, transform.position);
                }
                if(lockedDialogue != null){
                    DialogueManager.Instance.StartDialogue(lockedDialogue);
                }
                return;
            }

        // Open the door
        isOpen = true;
        isLocked = false;

        if (openSound != null)
        {
            AudioSource.PlayClipAtPoint(openSound, transform.position);
        }

        // Finish the current objective if the door is meant to
        if (completesObjective)
        {
            ObjectiveHUD.Instance.CompleteObjective();
        }

        // TODO add the repair later
        /*
        // Show the next objective
        if (nextObjective != "")
        {
            ObjectiveHUD.Instance.SetObjective(nextObjective);
        }
        */
    }
}
