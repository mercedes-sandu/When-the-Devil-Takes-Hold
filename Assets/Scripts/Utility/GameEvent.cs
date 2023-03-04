using UnityEngine;

public static class GameEvent
{
    /// <summary>
    /// Handles the kill timer.
    /// </summary>
    public delegate void KillTimerHandler(int time);

    /// <summary>
    /// Handles the game ending.
    /// </summary>
    public delegate void GameOverHandler(bool won);

    /// <summary>
    /// Handles the hide timer.
    /// </summary>
    public delegate void DemonLeaveHandler();

    /// <summary>
    /// Handles the puzzle the player will return to after the hide timer ends.
    /// </summary>
    public delegate void NextPuzzleHandler(Object puzzle);

    /// <summary>
    /// Handles the ammo being gained by the player.
    /// </summary>
    public delegate void AmmoHandler(int ammoGained);

    /// <summary>
    /// Detects when the kill timer should be changed.
    /// </summary>
    public static event KillTimerHandler OnKillTimerChange;

    /// <summary>
    /// Detects when the game is over.
    /// </summary>
    public static event GameOverHandler OnGameOver;
    
    /// <summary>
    /// Detects when the hide timer is finished.
    /// </summary>
    public static event DemonLeaveHandler OnDemonLeave;

    /// <summary>
    /// Detects when the next puzzle should be loaded after the hide timer is up.
    /// </summary>
    public static event NextPuzzleHandler OnNextPuzzle;
    
    /// <summary>
    /// Detects when the player has gained ammo.
    /// </summary>
    public static event AmmoHandler OnAmmoGain;

    /// <summary>
    /// Increases/decreases the maximum kill timer duration (in seconds).
    /// </summary>
    /// <param name="time">The new timer modification (in seconds), positive being an increase and negative being
    /// a decrease.</param>
    public static void ChangeKillTimerDuration(int time) => OnKillTimerChange?.Invoke(time);

    /// <summary>
    /// Ends the game based on whether the player won or lost.
    /// </summary>
    /// <param name="won">True if the player won, false otherwise.</param>
    public static void EndGame(bool won) => OnGameOver?.Invoke(won);
    
    /// <summary>
    /// Transitions to the next puzzle after the hide timer is up.
    /// </summary>
    public static void DemonLeft() => OnDemonLeave?.Invoke();

    /// <summary>
    /// Sets the puzzle the player failed to complete when the hide timer is up.
    /// </summary>
    /// <param name="puzzle">The puzzle to be loaded.</param>
    public static void SetNextPuzzle(Object puzzle) => OnNextPuzzle?.Invoke(puzzle);
    
    /// <summary>
    /// Adds the specified amount of ammo to the player's weapon.
    /// </summary>
    /// <param name="ammoGained">The amount of ammo the player gained.</param>
    public static void GainAmmo(int ammoGained) => OnAmmoGain?.Invoke(ammoGained);
}