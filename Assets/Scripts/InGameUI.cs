using UnityEngine;

public class InGameUI : MonoBehaviour
{
    /// <summary>
    /// The pause menu canvas.
    /// </summary>
    [SerializeField] private Canvas pauseMenu;
    
    /// <summary>
    /// True if the game is paused, false otherwise
    /// </summary>
    private bool _paused = false;

    /// <summary>
    /// Disable the pause menu.
    /// </summary>
    private void Start()
    {
        pauseMenu.enabled = false;
    }

    /// <summary>
    /// Pauses the game when the player presses the escape key.
    /// </summary>
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        pauseMenu.enabled = !_paused;
        Time.timeScale = _paused ? 1 : 0;
        _paused = !_paused;
    }
}