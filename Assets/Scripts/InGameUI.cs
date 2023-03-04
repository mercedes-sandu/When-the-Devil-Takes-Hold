using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    public static InGameUI Instance = null;
    
    /// <summary>
    /// The pause menu canvas.
    /// </summary>
    [SerializeField] private Canvas pauseMenu;

    /// <summary>
    /// The kill timer text object.
    /// </summary>
    [SerializeField] private TextMeshProUGUI killTimer;

    /// <summary>
    /// The kill timer duration.
    /// </summary>
    [SerializeField] private int killTimerDuration = 60;

    /// <summary>
    /// The scene to be loaded when the kill timer is up.
    /// </summary>
    [SerializeField] private Object scene;

    /// <summary>
    /// True if the kill timer is allowed in this scene, false otherwise.
    /// </summary>
    [SerializeField] private bool killTimerAllowed = true; // todo: remove

    /// <summary>
    /// The amount of seconds left on the kill timer.
    /// </summary>
    private int _secondsLeft;
    
    /// <summary>
    /// True if the game is paused, false otherwise
    /// </summary>
    private bool _paused = false;
    
    /// <summary>
    /// The animator component for the screen that fades to black.
    /// </summary>
    private Animator _fadeToBlack;

    /// <summary>
    /// The last coroutine which occurred.
    /// </summary>
    private Coroutine _lastCoroutine = null;

    /// <summary>
    /// Subscribes to game events and initializes components.
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
        
        _fadeToBlack = GetComponent<Animator>();
        
        GameEvent.OnKillTimerChange += ChangeKillTimerDuration;
    }
    
    /// <summary>
    /// Disables the pause menu and starts the kill timer countdown.
    /// </summary>
    private void Start()
    {
        pauseMenu.enabled = false;
        if (!killTimerAllowed) return;
        _secondsLeft = killTimerDuration + MainManager.Instance.killTimerModifier;
        Debug.Log("Kill timer initialized with " + _secondsLeft + " seconds.");
        UpdateTimerText();
        _lastCoroutine = StartCoroutine(Countdown());
    }

    /// <summary>
    /// Pauses the game when the player presses the escape key.
    /// </summary>
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        SwitchGameState();
    }

    /// <summary>
    /// Pauses/unpauses the game.
    /// </summary>
    public void SwitchGameState()
    {
        pauseMenu.enabled = !_paused;
        Time.timeScale = _paused ? 1 : 0;
        _paused = !_paused;
    }

    /// <summary>
    /// Quits the game.
    /// </summary>
    public void QuitButton()
    {
        Application.Quit();
    }

    /// <summary>
    /// Makes the kill timer count down and transition to the fight scene when the timer is up.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Countdown()
    {
        while (_secondsLeft > 0)
        {
            yield return new WaitForSeconds(1);
            _secondsLeft--;
            UpdateTimerText();
        }
        
        UpdateTimerText();
        TransitionToFightScene();
    }

    /// <summary>
    /// Updates the kill timer text with the specified amount of time left (in seconds).
    /// </summary>
    private void UpdateTimerText()
    {
        string minutesLeft = Mathf.FloorToInt(_secondsLeft / 60).ToString();
        string seconds = Mathf.FloorToInt(_secondsLeft % 60).ToString();
        seconds = seconds.Length == 1 ? "0" + seconds : seconds;
        killTimer.text = minutesLeft + ":" + seconds;
    }

    /// <summary>
    /// Increases/decreases the maximum kill timer duration (in seconds).
    /// </summary>
    /// <param name="time">The new timer modification (in seconds), positive being an increase and negative being
    /// a decrease.</param>
    private void ChangeKillTimerDuration(int time)
    {
        StopCoroutine(_lastCoroutine);
        killTimerDuration += time;
        _secondsLeft += time;
        if (_secondsLeft > 0) _lastCoroutine = StartCoroutine(Countdown());
    }

    /// <summary>
    /// Plays the fade to black animation when the player's kill timer runs out.
    /// </summary>
    private void TransitionToFightScene()
    {
        _fadeToBlack.Play("FadeToBlack");
    }

    /// <summary>
    /// Loads the specified scene, called by the fade to black animator.
    /// </summary>
    private void LoadScene()
    {
        SceneManager.LoadScene(scene.name);
    }

    /// <summary>
    /// Unsubscribes from game events.
    /// </summary>
    private void OnDestroy()
    {
        GameEvent.OnKillTimerChange -= ChangeKillTimerDuration;
    }
}