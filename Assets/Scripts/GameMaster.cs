using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    /// <summary>
    /// The win scene.
    /// </summary>
    [SerializeField] private Object winScene;

    /// <summary>
    /// The lose scene.
    /// </summary>
    [SerializeField] private Object loseScene;

    /// <summary>
    /// Subscribes to game events.
    /// </summary>
    private void Awake()
    {
        GameEvent.OnGameOver += GameOver;
    }

    /// <summary>
    /// Changes the scene to the game over scene.
    /// </summary>
    /// <param name="won">True if the player won, false otherwise.</param>
    private void GameOver(bool won)
    {
        SceneManager.LoadScene(won ? winScene.name : loseScene.name);
    }

    /// <summary>
    /// Unsubscribes from game events.
    /// </summary>
    private void OnDestroy()
    {
        GameEvent.OnGameOver -= GameOver;
    }
}