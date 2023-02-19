using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public GameObject loadingScreen;    // The loading screen UI object
    public Slider loadingBar;           // The loading bar UI object

    // Plays the "Theme" sound using the AudioManager script.
    void Start()
    {
        FindObjectOfType<AudioManager>().PlaySound("Theme");
    }

    // Loads the specified level asynchronously
    public void LoadLevel(int levelIndex)
    {
        StartCoroutine(LoadSceneAsynchronously(levelIndex));
    }

    // Coroutine that loads the scene asynchronously while updating the loading bar UI
    IEnumerator LoadSceneAsynchronously(int levelIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelIndex);
        while (!operation.isDone)
        {
            loadingBar.value = operation.progress;
            loadingScreen.SetActive(true);
            yield return null;
        }
    }

    // Quits the game
    public void QuitGame()
    {
        Application.Quit();
    }
}