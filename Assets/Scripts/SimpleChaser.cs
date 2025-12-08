using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    Chasing
}

[RequireComponent(typeof(NavMeshAgent))]
public class SimpleChaser : MonoBehaviour
{

    [Header("References")]
    public Transform target;

    [Header("Chase settings")]
    public float chaseRange = 10f;

    private NavMeshAgent agent;
    private EnemyState currentState = EnemyState.Idle;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            return;
        }

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        switch (currentState)
        { 
            case EnemyState.Idle:
                UpdateIdle(distanceToTarget); 
                break;
            case EnemyState.Chasing:
                UpdateChasing(distanceToTarget); 
                break;
        }
    }

    void UpdateIdle(float distanceToTarget)
    {
        if (distanceToTarget <= chaseRange) 
        {
            currentState = EnemyState.Chasing;
        }
    }

    void UpdateChasing(float distanceToTarget)
    {
        agent.SetDestination(target.position);
        if (distanceToTarget > chaseRange)
        {
            currentState = EnemyState.Idle;
            agent.ResetPath();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
