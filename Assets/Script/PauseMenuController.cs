using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // For buttons

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuUI; // Reference to the Pause Menu UI
    public GameObject boatController; // Reference to the Boat Controller to stop movement when paused

    private bool isGamePaused = false; // Flag to track if the game is paused

    void Update()
    {
        // Toggle pause menu when pressing "Escape"
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // Hide the pause menu
        Time.timeScale = 1f; // Resume the game time
        isGamePaused = false; // Set the game as not paused
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true); // Show the pause menu
        Time.timeScale = 0f; // Freeze the game time
        isGamePaused = true; // Set the game as paused
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Ensure time resumes before restarting
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene to restart the game
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop the game in the editor
        #else
        Application.Quit(); // Quit the game when running as a build
        #endif
    }
}
