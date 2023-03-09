using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    /// <summary>
    /// The instance of the GameMaster accessible by all classes.
    /// </summary>
    public static GameMaster Instance = null;
    
    /// <summary>
    /// The win scene.
    /// </summary>
    [SerializeField] private Object winScene;

    /// <summary>
    /// The lose scene.
    /// </summary>
    [SerializeField] private Object loseScene;

    /// <summary>
    /// The starting puzzle scene.
    /// </summary>
    [SerializeField] private Object startingPuzzle;

    /// <summary>
    /// The next puzzle to be loaded when the player successfully hides during the fight stage.
    /// </summary>
    private Object _nextPuzzle;

    /// <summary>
    /// Subscribes to game events.
    /// </summary>
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
        
#if UNITY_EDITOR
        GameEvent.OnNextPuzzle += SetNextPuzzle;
#endif
        GameEvent.OnGameOver += GameOver;
    }

    /// <summary>
    /// Sets next puzzle to the starting puzzle.
    /// </summary>
    private void Start()
    {
        _nextPuzzle = startingPuzzle;
    }

    /// <summary>
    /// Sets the next puzzle the player will return to after the hide timer ends.
    /// </summary>
    /// <param name="puzzle"></param>
    private void SetNextPuzzle(Object puzzle)
    {
        _nextPuzzle = puzzle;
    }

    /// <summary>
    /// Loads the specified scene, called by the fade to black animator.
    /// </summary>
    public void LoadScene()
    {
        SceneManager.LoadScene(_nextPuzzle.name);
    }
    
    /// <summary>
    /// Changes the scene to the game over scene.
    /// </summary>
    /// <param name="won">True if the player won, false otherwise.</param>
    private void GameOver(bool won)
    {
        MainManager.Instance.health = 100;
        MainManager.Instance.ammo = 75;
        MainManager.Instance.killTimerModifier = 0;
#if UNITY_EDITOR
        SceneManager.LoadScene(won ? winScene.name : loseScene.name);
#else
        SceneManager.LoadScene(won ? "GameOverWin" : "GameOverLose");
#endif
    }

    /// <summary>
    /// Unsubscribes from game events.
    /// </summary>
    private void OnDestroy()
    {
#if UNITY_EDITOR
        GameEvent.OnNextPuzzle -= SetNextPuzzle;
#endif
        GameEvent.OnGameOver -= GameOver;
    }
}