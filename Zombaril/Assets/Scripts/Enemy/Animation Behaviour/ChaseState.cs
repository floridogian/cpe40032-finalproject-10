using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : StateMachineBehaviour
{
    // Declare variables
    NavMeshAgent agent;         // For controlling navigation
    Transform player;           // For tracking the player's position
    Enemy enemy;                // Reference to the enemy script attached to the object
    AudioManager audioManager;  // Reference to the AudioManager script in the scene

    // Attack-related variables
    private float timeOfLastAttack = 0;   // Time of the last attack
    private float attackSpeed = 1.5f;     // Time between attacks
    private float attackRange = 2.8f;     // Distance at which the enemy will attack
    private bool isAttacking = false;     // Whether the enemy is currently attacking

    // Called when the state is first entered
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Get references to necessary components
        agent = animator.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = animator.GetComponent<Enemy>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    // Called every frame while the state is active
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Move towards the player
        agent.SetDestination(player.position);

        // Check if the player is within attack range
        float distance = Vector3.Distance(animator.transform.position, player.position);
        if (distance <= attackRange)
        {
            // If the enemy is not already attacking, set isAttacking to true and play the attacking sound
            if (!isAttacking)
            {
                isAttacking = true;
                audioManager.PlaySound("Attacking");
            }

            // Check if enough time has passed since the last attack, and if so, trigger the attack animation and call the enemy's Attack method
            if (Time.time >= timeOfLastAttack + attackSpeed)
            {
                timeOfLastAttack = Time.time;
                animator.SetTrigger("isAttacking");
                enemy.Attack();
            }
        }
        else
        {
            // If the player is not within attack range, set isAttacking to false
            isAttacking = false;
        }
    }

    // Called when the state is exited
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Stop moving
        agent.SetDestination(agent.transform.position);
    }
}