using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Public variables
    public GameObject gameOverScreen;   // A reference to the game over screen object
    public GameObject winScreen;        // A reference to the win screen object
    public GameObject playerCanvas;     // A reference to the player canvas object
    public GameObject enemy;            // A reference to the enemy object
    public GameObject player;           // A reference to the player object
    public GameObject pauseScreen;      // A reference to the pause screen object
    public Slider volumeSlider;         // A reference to the volume slider
    public GameObject loadingScreen;    // A reference to loading screen
    public Slider loadingBar;           // A reference to loading bar
    
    // Declare audio variable 
    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        // Load the player's preferred volume setting or use the default value
        volumeSlider.value = PlayerPrefs.GetFloat("currentVolume", volumeSlider.value);

        // Get the audio manager component in the scene
        audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Save the current volume setting to PlayerPrefs
        PlayerPrefs.SetFloat("currentVolume", volumeSlider.value);

        // Pause the game when the Escape key is pressed
        if (Input.GetKey(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    // Adjust the volume of the game's audio
    public void SetVolume()
    {
        AudioListener.volume = volumeSlider.value;
    }

    // This method is called when the player loses the game
    public void GameOver()
    {
        // Display the game over screen
        gameOverScreen.SetActive(true);

        // Hide the player canvas
        playerCanvas.SetActive(false);

        // Destroy the enemy object after a short delay
        Destroy(enemy, 0.1f);

        // Unlock the cursor to allow the player to interact with the game over screen
        Cursor.lockState = CursorLockMode.None;

        // Play the "game over" sound effect using the audio manager
        audioManager.PlaySound("GameOver");

        // Disable all the scripts on the player and its children to prevent movement or interaction with the game world
        MonoBehaviour[] scripts = player.GetComponentsInChildren<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = false;
        }

        // Drop any carried items using the PickUpController script on the player object
        PickUpController pickUpController = player.GetComponentInChildren<PickUpController>();
        pickUpController.Drop();
    }

    // This method is called when the player wins the game
    public void WinGame()
    {
        // Display the win screen
        winScreen.SetActive(true);

        // Hide the player canvas
        playerCanvas.SetActive(false);

        // Unlock the cursor to allow the player to interact with the win screen
        Cursor.lockState = CursorLockMode.None;

        // Play the "win" sound effect using the audio manager
        audioManager.PlaySound("GameOver");

        // Disable all the scripts on the player and its children to prevent movement or interaction with the game world
        MonoBehaviour[] scripts = player.GetComponentsInChildren<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = false;
        }
    }

    // This method is called when the player chooses to restart the game from the game over screen
    public void RestartGame()
    {
        StartCoroutine(RestartSceneAsynchronously()); // Reload the current scene asynchronously
    }

    // This method is called when the player chooses to return to the main menu from the game over or win screens
    public void MainMenu()
    {
        Time.timeScale = 1; // Unpause the game
        StartCoroutine(LoadSceneAsynchronously()); // Load the main menu scene asynchronously
    }

    // This method is called when the player chooses to quit the game from the game over or win screens
    public void QuitGame()
    {
        Application.Quit(); // Quit the game
    }

    // This method is called when the player chooses to pause the game
    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Time.timeScale = 0; // Pause the game
        playerCanvas.SetActive(false); // Disable the player canvas
        pauseScreen.SetActive(true); // Enable the pause screen
    }

    // This method is called when the player chooses to resume the game
    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        Time.timeScale = 1; // Unpause the game
        playerCanvas.SetActive(true); // Enable the player canvas
        pauseScreen.SetActive(false); // Disable the pause screen
    }

    // Load the next level when the player press continue
    public void ContinueGame()
    {
        int nextLevel = SceneManager.GetActiveScene().buildIndex + 1; // Get the index of the next level

        if (nextLevel == 4) // If the next level is the last level
        {
            SceneManager.LoadScene(0); // Load the main menu scene
        }

        if (PlayerPrefs.GetInt("UnlockedLevel", 1) < nextLevel) // If the next level is greater than the unlocked level
        {
            PlayerPrefs.SetInt("UnlockedLevel", nextLevel); // Unlock the next level
        }

        SceneManager.LoadScene(nextLevel); // Load the next level
    }

    // Load the main menu scene asynchronously
    IEnumerator LoadSceneAsynchronously()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(0); // Load the main menu scene asynchronously
        while (!operation.isDone) // While the scene is not loaded
        {
            loadingBar.value = operation.progress; // Update the loading bar with the progress
            loadingScreen.SetActive(true); // Enable the loading screen
            yield return null; // Wait for the next frame
        }
    }

    // Reload the current scene asynchronously
    IEnumerator RestartSceneAsynchronously()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name); // Reload the current scene asynchronously
        while (!operation.isDone) // While the scene is not loaded
        {
            loadingBar.value = operation.progress; // Update the loading bar with the progress
            loadingScreen.SetActive(true); // Enable the loading screen
            yield return null; // Wait for the next frame
        }
    }
}
