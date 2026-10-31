using UnityEngine;

public class HideInteractable : MonoBehaviour
{
    private Animator animation;
    private bool isHiding;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake(){
        animation = GetComponent<Animator>();
    }

    //check if the player is trying to hide if so play the locker opening animation
    private void ToggleHide(){
        isHiding = !isHiding;
        animation.SetBool("IsHiding", isHiding);
    }
}
