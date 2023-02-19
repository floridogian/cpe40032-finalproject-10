using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    GameObject bossEnemy;     // Reference to the boss enemy game object
    GameManager gameManager;  // Reference to the game manager script

    public GameObject killedBoss;  // Display text saying the player killed the boss

    // Start is called before the first frame update
    void Start()
    {
        // Find the boss enemy game object with the "BossEnemy" tag
        bossEnemy = GameObject.FindGameObjectWithTag("BossEnemy");

        // Get the game manager script
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        // If the boss is dead, display text
        if (bossEnemy == null)
        {
            killedBoss.SetActive(true);
        }
    }

    // Called when another collider enters this trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // If the other collider has the "Player" tag and the boss enemy has been defeated (i.e. is null)
        if (other.tag == "Player" && bossEnemy == null)
        {
            // Call the WinGame method on the game manager and disable text
            killedBoss.SetActive(false);
            gameManager.WinGame();
        }
    }
}
