using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Character Settings
    public CharacterController controller;
    public Transform groundCheck;
    public LayerMask groundMask;
    public float groundDistance = 0.1f;

    // Movement Settings
    public float speed = 6f;
    public float gravity = -20f;
    public float jumpHeight = 0.35f;

    // Health Settings
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthBar;
    public GameObject bloodScreenOverlay;
    public GameObject healthScreenOverlay;

    // Animation Settings
    public Animator animator;

    // Audio Settings
    public float walkingSoundInterval = 0.5f;

    // Other Settings
    public GameManager gameManager;
    private bool isGameOver;

    private Vector3 velocity;
    private bool isGrounded;
    private float timeSinceLastWalkingSound;

    // Start function is called before the first frame update
    void Start()
    {
        // Initialize the player's health
        currentHealth = maxHealth;
        // Initialize the game over to false
        isGameOver = false;
    }

    // Update function is called once per frame
    void Update()
    {
        UpdateHealthText();
        CheckGrounded();
        Move();
        ApplyGravity();
        Jump();
    }

    private void UpdateHealthText()
    {
        // Update the health of the player's health
        healthBar.value = currentHealth;
    }

    private void Move()
    {
        // Get the horizontal and vertical input axis values
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Determine if the player is currently walking
        bool isWalking = Mathf.Abs(x) + Mathf.Abs(z) > 0;

        // Change gun animation from idle to walk
        animator.SetFloat("speed", isWalking ? 1.0f : 0.0f);

        // Calculate the movement vector based on the input axis values
        Vector3 move = transform.right * x + transform.forward * z;

        // Move the player based on the calculated movement vector
        controller.Move(move * speed * Time.deltaTime);

        // Only play walking sound at certain intervals
        if (isWalking && Time.time - timeSinceLastWalkingSound > walkingSoundInterval)
        {
            FindObjectOfType<AudioManager>().PlaySound("Walking");
            timeSinceLastWalkingSound = Time.time;
        }
    }

    private void Jump()
    {
        // If the player presses the jump button and is on the ground
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Calculate the y-component of the velocity required for a jump of the specified height
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            FindObjectOfType<AudioManager>().PlaySound("Jump");
        }
    }

    private void CheckGrounded()
    {
        // Check if the player is on the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // If the player is on the ground and the y-component of their velocity is negative
        if (isGrounded && velocity.y < 0)
        {
            // Reset the y-component of the velocity to 0
            velocity.y = -2f;
        }
    }

    private void ApplyGravity()
    {
        // Apply gravity to the y-component of the velocity
        velocity.y += gravity * Time.deltaTime;

        // Move the player based on their velocity
        controller.Move(velocity * Time.deltaTime);
    }

    // Function to increase the player's health by a specified amount
    public void GainHealth(int amount)
    {
        // Check if the amount is greater than 0
        if (amount > 0)
        {
            // Increment the current health by the amount
            currentHealth += amount;

            // Make sure current health doesn't exceed max health
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            StartCoroutine(HealthOverlay());
        }
    }

    // Function for applying damage to the player
    public void TakeDamage(int damage)
    {
        // Decrement the player's health by the specified amount of damage
        FindObjectOfType<AudioManager>().PlaySound("Damaged");
        currentHealth -= damage;
        StartCoroutine(BloodOverlay());

        // If the player's health is less than or equal to 0
        if (currentHealth <= 0 && !isGameOver)
        {
            Die();
        }
    }

    // Function for player to die
    void Die()
    {
        isGameOver = true;
        gameManager.GameOver();
    }

    IEnumerator BloodOverlay()
    {
        bloodScreenOverlay.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        bloodScreenOverlay.SetActive(false);
    }

    IEnumerator HealthOverlay()
    {
        healthScreenOverlay.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        healthScreenOverlay.SetActive(false);
    }
}
