using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : StateMachineBehaviour
{
    // Declare variable
    AudioManager audioManager;  // Reference to the AudioManager script in the scene

    // Private variables
    private float timer;               // The time the enemy stays in idle state before going into patrolling
    private float timeToPatrol = 10.0f;        // Waiting time for enemy to patrol
    private float chaseRange = 15.0f;  // The range in which the enemy can chase the player
    private Transform player;          // The player game object


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Reset the timer when entering the idle state
        timer = 0;

        // Get references to necessary components
        player = GameObject.FindGameObjectWithTag("Player").transform;
        audioManager = FindObjectOfType<AudioManager>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Increase the timer as long as the enemy is in idle state
        timer += Time.deltaTime;

        // If the timer reaches a certain threshold, switch to patrolling state and play patrolling sound effect
        if (timer > timeToPatrol)
        {
            animator.SetBool("isPatrolling", true);
            audioManager.PlaySound("Patrolling");
        }

        // Calculate the distance between the enemy and the player
        float distance = Vector3.Distance(animator.transform.position, player.position);

        // If the player is within chase range, switch to chasing state
        if (distance < chaseRange)
        {
            animator.SetBool("isChasing", true);
            audioManager.PlaySound("Chasing");
        }
    }
}