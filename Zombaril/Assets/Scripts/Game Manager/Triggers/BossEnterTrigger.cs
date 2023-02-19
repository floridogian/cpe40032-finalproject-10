using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnterTrigger : MonoBehaviour
{
    // Public variables for the closed and opened rotations of the doors
    public Quaternion doorOneClosedRotation = Quaternion.Euler(270f, 270f, 0f);
    public Quaternion doorTwoClosedRotation = Quaternion.Euler(270f, 270f, 0f);
    public Quaternion doorOneOpenedRotation = Quaternion.Euler(270f, 180f, 0f);
    public Quaternion doorTwoOpenedRotation = Quaternion.Euler(270f, 1f, 0f);

    // The transforms of the two doors that need to be opened/closed
    public Transform doorOne;
    public Transform doorTwo;

    // The speed at which the doors open/close
    public float openSpeed = 5.0f;

    // The text that warns the player to defeat all enemies before entering
    public GameObject warningText;
    public GameObject eliminateText;

    // Private variable for the current state of the doors (open or closed)
    private bool isOpen = false;

    // The initial count of enemies in the scene
    private int enemyCount;

    // Start is called before the first frame update
    void Start()
    {
        // Get the initial count of enemies in the scene
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    // Method to decrease the enemy count when an enemy is defeated
    public void DecreaseEnemyCount()
    {
        enemyCount--;
    }

    // Check for player entering the trigger area
    private void OnTriggerEnter(Collider other)
    {
        // If the player enters and all enemies are defeated
        if (other.tag == "Player" && enemyCount == 0)
        {
            warningText.SetActive(true);
        }
        else if (other.tag == "Player" && enemyCount > 0)
        {
            eliminateText.SetActive(true);
        }
    }

    // Check for player exiting the trigger area
    private void OnTriggerExit(Collider other)
    {
        // If the player exits, hide the warning text and close the doors
        if (other.tag == "Player")
        {
            warningText.SetActive(false);
            eliminateText.SetActive(false);
            CloseDoors();
        }
    }

    // Method to open both doors
    public void OpenDoors()
    {
        isOpen = true;
    }

    // Method to close both doors
    public void CloseDoors()
    {
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        // If the doors are open, gradually rotate them to the open position
        if (isOpen)
        {
            doorOne.rotation = Quaternion.Lerp(doorOne.rotation, doorOneOpenedRotation, Time.deltaTime * openSpeed);
            doorTwo.rotation = Quaternion.Lerp(doorTwo.rotation, doorTwoOpenedRotation, Time.deltaTime * openSpeed);
        }
        // If the doors are closed, gradually rotate them to the closed position
        else
        {
            doorOne.rotation = Quaternion.Lerp(doorOne.rotation, doorOneClosedRotation, Time.deltaTime * openSpeed);
            doorTwo.rotation = Quaternion.Lerp(doorTwo.rotation, doorTwoClosedRotation, Time.deltaTime * openSpeed);
        }

        // If the player presses the 'E' key while the warning text is active and all enemies are defeated, open the doors
        if (Input.GetKeyDown(KeyCode.E) && warningText.activeSelf && enemyCount == 0)
        {
            OpenDoors();
        }
    }
}