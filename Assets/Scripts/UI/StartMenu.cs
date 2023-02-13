using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    /// <summary>
    /// Starts the game.
    /// </summary>
    public void StartButton()
    {
        SceneManager.LoadScene("PuzzleOne");
    }

    /// <summary>
    /// Quits the game.
    /// </summary>
    public void QuitButton()
    {
        Application.Quit();
    }

    /// <summary>
    /// Loads the start menu.
    /// </summary>
    public void ReplayButton()
    {
        SceneManager.LoadScene("StartMenu");
    }
}