using UnityEngine;

public class FightFadeToBlack : MonoBehaviour
{
    /// <summary>
    /// Loads the puzzle level the player was working on before the fight scene started. Called by the animator.
    /// </summary>
    private void LoadPuzzleLevel()
    {
        GameMaster.Instance.LoadScene();
    }
}