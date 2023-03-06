using UnityEngine;
using UnityEngine.SceneManagement;

public class Instructions : MonoBehaviour
{
    /// <summary>
    /// The instructions canvas.
    /// </summary>
    [SerializeField] private Canvas instructionsCanvas;

    /// <summary>
    /// The (current) story canvas.
    /// </summary>
    private Canvas _storyCanvas;

    /// <summary>
    /// Initializes components.
    /// </summary>
    private void Awake()
    {
        _storyCanvas = GetComponent<Canvas>();
        _storyCanvas.enabled = true;
        instructionsCanvas.enabled = false;
    }
    
    /// <summary>
    /// Disables the story canvas and enables the instructions canvas.
    /// </summary>
    public void NextButton()
    {
        instructionsCanvas.enabled = true;
        _storyCanvas.enabled = false;
    }
    
    /// <summary>
    /// Starts the game at the first puzzle.
    /// </summary>
    public void StartButton()
    {
        SceneManager.LoadScene("PuzzleOne");
    }
}