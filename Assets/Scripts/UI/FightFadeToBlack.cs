using UnityEngine;
using UnityEngine.SceneManagement;

public class FightFadeToBlack : MonoBehaviour
{
    /// <summary>
    /// Loads the puzzle level the player was working on before the fight scene started. Called by the animator.
    /// </summary>
    private void LoadPuzzleLevel()
    {
#if UNITY_EDITOR
        GameMaster.Instance.LoadScene();
#else
        SceneManager.LoadScene(MainManager.Instance.currentPuzzle);
#endif
    }
}