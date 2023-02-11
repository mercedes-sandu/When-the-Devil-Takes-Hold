using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    /// <summary>
    /// Starts the game.
    /// </summary>
    public void StartButton()
    {
        SceneManager.LoadScene("DemoScene");
    }

    /// <summary>
    /// Quits the game.
    /// </summary>
    public void QuitButton()
    {
        Application.Quit();
    }
}