using UnityEngine;

public abstract class EnemyAIController : MonoBehaviour
{
    protected UnityEngine.AI.NavMeshAgent Agent {get; private set;}
    //ANIMATION CONTROLLER GOES HERE IN THE FUTURE
    protected Transform[] WayPoints;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
