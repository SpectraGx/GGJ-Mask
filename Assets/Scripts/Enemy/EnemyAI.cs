using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum State { Patrol, Chase, Alerted, Stunned }
    public State currentState = State.Patrol;

    [Header("Patrol Settings")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float visionRange = 5f;
    [SerializeField] private float loseRange = 7f;
    [SerializeField] private LayerMask obstacleLayer;

    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;

    private int currentPatrolIndex = 0;
    private Vector3 lastPatrolPosition;

    void Start()
    {
        agent=GetComponent<NavMeshAgent>();

        if (player == null) { player = GameObject.FindGameObjectWithTag("Player").transform; }

        GoNextPatrolPoint();
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Patrol:
                PatrolLogic();
                CheckForPlayer();
                break;
            case State.Chase:
                ChaseLogic();
                break;
            case State.Alerted:
                AlertedLogic();
                break;
            case State.Stunned:
                // Do nothing while stunned
                break;
        }
    }

    private void PatrolLogic()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoNextPatrolPoint();
        }
    }

    private void ChaseLogic()
    {
        agent.SetDestination(player.position);

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > loseRange)
        {
            ReturnToPatrol();
        }
    }

    private void AlertedLogic()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            ReturnToPatrol();
        }

        CheckForPlayer();
    }

    void CheckForPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= visionRange)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;

            if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleLayer))
            {
                StartChasing();
            }
        }
    }

    void StartChasing()
    {
        lastPatrolPosition = transform.position;
        currentState = State.Chase;
    }

    void ReturnToPatrol()
    {
        currentState = State.Patrol;
        GoNextPatrolPoint();
    }

    void GoNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        agent.destination = patrolPoints[currentPatrolIndex].position;
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    public void SetAlertedState(Vector3 location)
    {
        Debug.Log("Enemy alerted to location: " + location);
        currentState = State.Alerted;
        agent.SetDestination(location);
    }

    public void StunEnemy(float duration)
    {
        StartCoroutine(StunCoroutine(duration));
    }

    IEnumerator StunCoroutine(float duration)
    {
        agent.isStopped = true;

        agent.ResetPath();

        State previousState = currentState;
        currentState = State.Stunned;

        Debug.Log("Enemy stunned");

        yield return new WaitForSeconds(duration);

        agent.isStopped = false;
        currentState = previousState;

        if (currentState == State.Patrol) GoNextPatrolPoint();

        Debug.Log("Enemy recovered from stun");
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, loseRange);
    }
}
