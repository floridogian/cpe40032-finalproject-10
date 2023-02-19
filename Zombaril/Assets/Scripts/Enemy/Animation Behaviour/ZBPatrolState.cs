using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZBPatrolState : StateMachineBehaviour
{
    // Declare variables
    List<Transform> wayPoints = new List<Transform>();  // A list of transforms representing the patrol points
    NavMeshAgent agent;                                 // The agent that controls the enemy's movement
    Transform player;                                   // The player's transform
    AudioManager audioManager;                          // Reference to the AudioManager script in the scene

    // Private variables
    private float patrolTime = 10.0f;     // The amount of time this state should last before transitioning to another state
    private float chaseRange = 5.0f;     // The maximum distance the enemy can see the player and start chasing them
    private float timeToIdle = 5.0f;     // Waiting time for enemy to stop patrolling and go on idle state

    // Called when the state is entered
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Reset the patrol timer
        patrolTime = 0f;

        // Find all the patrol points and add them to the list
        Transform wayPointsObject = GameObject.FindGameObjectWithTag("ZBWayPoints").transform;
        foreach (Transform t in wayPointsObject)
        {
            wayPoints.Add(t);
        }

        // Get the agent component and set its destination to a random patrol point
        agent = animator.GetComponent<NavMeshAgent>();
        agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);

        // Get references to necessary components
        player = GameObject.FindGameObjectWithTag("Player").transform;
        audioManager = FindObjectOfType<AudioManager>();
    }

    // Called on each frame while in this state
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // If the agent has reached its destination, set a new random destination from the patrol points
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
        }

        // Increment the patrol timer and transition to idle state if it exceeds the maximum patrol time
        patrolTime += Time.deltaTime;
        if (patrolTime > timeToIdle)
        {
            animator.SetBool("isPatrolling", false);
        }

        // Check if the player is within the chase range and transition to the chase state if they are
        float distance = Vector3.Distance(animator.transform.position, player.position);
        if (distance < chaseRange)
        {
            animator.SetBool("isChasing", true);
            audioManager.PlaySound("Chasing");
        }
    }

    // Called when the state is exited
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Stop the agent from moving
        agent.SetDestination(agent.transform.position);
    }
}
