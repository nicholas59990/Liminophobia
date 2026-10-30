using UnityEngine;

public class HidingController : MonoBehaviour
{
    public GameObject hideText;
    public GameObject stopHideText;
    public GameObject player;
    public GameObject hidingPlayer;
    public EnemyAIController enemyScript;
    public Transform enemyTransform;
    bool canHide;
    bool isHiding;
    public float loseDistance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canHide = false;
        isHiding = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(canHide == true){
            if(Input.GetKeyDown(KeyCode.E)){
                hideText.SetActive(false);
                hidingPlayer.SetActive(true);
                float aggroDistance = Vector3.Distance(enemyTransform, normalPlayer.transform.position);
                if(aggroDistance > loseDistance){
                    if(enemyScript.chasing == true){
                        enemyScript.stopChase();
                    }
                }
            }
        }
        */
    }

    void OnTriggerStay(Collider other){
        if(other.CompareTag("MainCamera")){
            hideText.SetActive(true);
            canHide = true;
        }
    }

    void OnTriggerExit(Collider other){
        if(other.CompareTag("MainCamera")){
            hideText.SetActive(false);
            canHide = false;
        }
    }
}
