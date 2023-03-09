using UnityEngine;

public class MainManager : MonoBehaviour
{
    /// <summary>
    /// The public instance of the MainManager, accessible by all classes.
    /// </summary>
    public static MainManager Instance;

    /// <summary>
    /// The amount of ammo the player has.
    /// </summary>
    public int ammo;

    /// <summary>
    /// The amount of health the player has.
    /// </summary>
    public int health;

    /// <summary>
    /// The amount of time to modify the kill timer by. Positive values add to the time, negative values subtract from
    /// the time.
    /// </summary>
    public int killTimerModifier;

    /// <summary>
    /// The puzzle the player is currently working on/has not finished.
    /// </summary>
    public string currentPuzzle;
    
    /// <summary>
    /// Sets the instance and makes it persist between scenes.
    /// </summary>
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}