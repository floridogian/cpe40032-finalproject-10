using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Enemy : MonoBehaviour
{
    // Enemy stats
    public int attackDamage; // Amount of damage the enemy will deal to the player when attacking
    public int maxHealth = 100;  // Maximum health of the enemy

    // Private variables
    private Transform player;         // Transform component of the player game object
    private int currentHealth;        // Current health of the enemy
    private Animator animator;        // Animator component of the enemy game object
    private BoxCollider boxCollider;  // BoxCollider component of the enemy game object
    private bool isDead = false;      // Flag indicating if the enemy is dead

    // Public variables
    public ParticleSystem bloodSplatter; // Particle system for blood splatter effect
    public GameObject health;            // Health game object used to display the current health of the enemy
    public Slider healthBar;             // Health bar used to display the current health of the enemy

    // Variables for audio and boss trigger
    AudioManager audioManager;
    BossEnterTrigger bossEnterTrigger;

    // Called when the game starts
    void Start()
    {
        // Find the player object by tag and get its transform component
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Set the current health to the maximum health
        currentHealth = maxHealth;

        // Get the Animator components
        animator = GetComponent<Animator>();

        // Get the AudioManager and BossEnterTrigger components
        audioManager = FindObjectOfType<AudioManager>();
        bossEnterTrigger = FindObjectOfType<BossEnterTrigger>();
    }

    // Called every frame
    void Update()
    {
        // Update the health bar to reflect the current health
        healthBar.value = currentHealth;
    }

    // Method called when the enemy takes damage
    public void TakeDamage(int damage)
    {
        // If the enemy is already dead, do nothing
        if (isDead) return;

        // Decrement the current health by the damage taken
        currentHealth -= damage;

        // If the current health is 0 or less, die
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Play damage and bloodsplatter effects, and set the isDamaged trigger on the animator
            bloodSplatter.Play();
            health.SetActive(true);
            audioManager.PlaySound("Damaged");
            animator.SetTrigger("isDamaged");
        }
    }

    // Method called when the enemy dies
    void Die()
    {
        // Stop the Chasing sound effect, set the isDead trigger on the animator, disable the box collider, and destroy the enemy game object
        isDead = true;
        audioManager.StopSound("Chasing");
        audioManager.PlaySound("Death");
        animator.SetTrigger("isDead");
        Destroy(gameObject, 2f);

        // Call the DecreaseEnemyCount method on the BossEnterTrigger script
        bossEnterTrigger.DecreaseEnemyCount();
    }

    // Method called when the enemy attacks
    public void Attack()
    {
        // Call the TakeDamage method on the player game object with the attackDamage value
        player.GetComponent<PlayerController>().TakeDamage(attackDamage);
    }
}