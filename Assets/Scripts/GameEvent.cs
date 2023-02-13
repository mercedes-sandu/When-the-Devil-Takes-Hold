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
    /// Detects when the kill timer should be changed.
    /// </summary>
    public static event KillTimerHandler OnKillTimerChange;

    /// <summary>
    /// Detects when the game is over.
    /// </summary>
    public static event GameOverHandler OnGameOver;

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
}