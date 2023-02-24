using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleMaster : MonoBehaviour
{
    /// <summary>
    /// The instance of the puzzle master accessible by all classes.
    /// </summary>
    public static PuzzleMaster Instance = null;
    
    /// <summary>
    /// The scene corresponding to the next puzzle.
    /// </summary>
    [SerializeField] private Object nextPuzzle;

    /// <summary>
    /// The NPCs to be killed for this puzzle.
    /// </summary>
    [SerializeField] private NPC[] npcs;

    /// <summary>
    /// True if the puzzle has another puzzle level after it, false otherwise.
    /// </summary>
    [SerializeField] private bool hasNextPuzzle = false;

    /// <summary>
    /// The number of NPCs the player needs to kill in this level.
    /// </summary>
    private int _npcsToKill;

    /// <summary>
    /// The number of NPCs the player has killed in this level.
    /// </summary>
    private int _npcsKilled;

    /// <summary>
    /// True if the puzzle has been solved, false otherwise.
    /// </summary>
    private bool _puzzleSolved = false;

    /// <summary>
    /// Initializes the instance.
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
    }

    /// <summary>
    /// Sets the number of NPCs to kill.
    /// </summary>
    private void Start()
    {
        _npcsToKill = npcs.Length;
    }

    /// <summary>
    /// Kills an NPC and updates according fields.
    /// </summary>
    public void KillNPC()
    {
        _npcsKilled++;
        if (_npcsKilled != _npcsToKill) return;
        _puzzleSolved = true;
        
        if (!hasNextPuzzle) return;
        GameEvent.SetNextPuzzle(nextPuzzle);
        Debug.Log("set next puzzle to " + nextPuzzle.name + " in PuzzleMaster");
        Invoke(nameof(LoadNextPuzzle), 1);
    }

    /// <summary>
    /// Loads the next puzzle.
    /// </summary>
    private void LoadNextPuzzle()
    {
        SceneManager.LoadScene(nextPuzzle.name);
    }
}