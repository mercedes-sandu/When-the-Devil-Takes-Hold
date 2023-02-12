public static class GameEvent
{
    /// <summary>
    /// Handles the kill timer.
    /// </summary>
    public delegate void KillTimerHandler(int time);

    /// <summary>
    /// Detects when the kill timer should be changed.
    /// </summary>
    public static event KillTimerHandler OnKillTimerChange;

    /// <summary>
    /// Increases/decreases the maximum kill timer duration (in seconds).
    /// </summary>
    /// <param name="time">The new timer modification (in seconds), positive being an increase and negative being
    /// a decrease.</param>
    public static void ChangeKillTimerDuration(int time) => OnKillTimerChange?.Invoke(time);
}