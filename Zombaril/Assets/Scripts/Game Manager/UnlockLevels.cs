using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockLevels : MonoBehaviour
{
    public Button[] buttonLevels;   // An array of buttons representing each level in the game

    // Start is called before the first frame update
    void Start()
    {
        // Disable all level buttons at the start of the game
        foreach (Button b in buttonLevels)
        {
            b.interactable = false;
        }

        // Get the highest level the player has unlocked from PlayerPrefs
        int levelUnlocked = PlayerPrefs.GetInt("UnlockedLevel", 1);

        // Enable buttons for all levels the player has unlocked
        for (int i = 0; i < levelUnlocked; i++)
        {
            buttonLevels[i].interactable = true;
        }
    }
}