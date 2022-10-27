using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This class provides functionality for the main menu
public class MainMenu : MonoBehaviour
{

    // This method is called when the "exit" button is pressed
    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Game Closed"); // Quit the game window
    }

    // This method is called when the "play" button is pressed
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene"); // Load the main game scene
    }
}