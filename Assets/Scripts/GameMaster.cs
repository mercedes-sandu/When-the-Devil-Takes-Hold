using System.Collections;
using TMPro;
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
    /// The hide timer text object.
    /// </summary>
    [SerializeField] private TextMeshProUGUI hideTimer;
    
    /// <summary>
    /// The next puzzle to be loaded when the player successfully hides during the fight stage.
    /// </summary>
    private Object _nextPuzzle;
    
    /// <summary>
    /// The amount of seconds left on the kill timer.
    /// </summary>
    private int _secondsLeft;
    
    /// <summary>
    /// The last coroutine which occurred.
    /// </summary>
    private Coroutine _lastCoroutine = null;

    /// <summary>
    /// The animator.
    /// </summary>
    private Animator _anim;
    
    /// <summary>
    /// True if a coroutine is running, false otherwise.
    /// </summary>
    private bool _coroutineRunning = false;

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
        
        _anim = GetComponent<Animator>();
        
        DontDestroyOnLoad(this);
        
        GameEvent.OnHideTimerStart += StartHideTimer;
        GameEvent.OnHideTimerStop += StopHideTimer;
        GameEvent.OnNextPuzzle += SetNextPuzzle;
        GameEvent.OnGameOver += GameOver;
    }

    /// <summary>
    /// Sets next puzzle to the starting puzzle.
    /// </summary>
    private void Start()
    {
        _nextPuzzle = startingPuzzle;
        hideTimer.enabled = false;
    }
    
    /// <summary>
    /// Starts the hide timer with the specified amount of time (in seconds).
    /// </summary>
    /// <param name="time">The amount of time it should take to hide (in seconds).</param>
    private void StartHideTimer(int time)
    {
        _secondsLeft = time;
        UpdateTimerText();
        hideTimer.enabled = true;
        _lastCoroutine = StartCoroutine(Countdown());
    }

    /// <summary>
    /// Stops the hide timer.
    /// </summary>
    private void StopHideTimer()
    {
        if (!_coroutineRunning) return;
        StopCoroutine(_lastCoroutine);
        hideTimer.enabled = false;
    }
    
    /// <summary>
    /// Makes the kill timer count down and transition to the fight scene when the timer is up.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Countdown()
    {
        _coroutineRunning = true;
        while (_secondsLeft > 0)
        {
            yield return new WaitForSeconds(1);
            _secondsLeft--;
            UpdateTimerText();
        }

        _coroutineRunning = false;
        UpdateTimerText();
        DemonLeaveScreen();
    }
    
    /// <summary>
    /// Updates the hide timer text with the specified amount of time left (in seconds).
    /// </summary>
    private void UpdateTimerText()
    {
        string minutesLeft = Mathf.FloorToInt(_secondsLeft / 60).ToString();
        string seconds = Mathf.FloorToInt(_secondsLeft % 60).ToString();
        seconds = seconds.Length == 1 ? "0" + seconds : seconds;
        hideTimer.text = minutesLeft + ":" + seconds;
    }
    
    /// <summary>
    /// Sets the next puzzle the player will return to after the hide timer ends.
    /// </summary>
    /// <param name="puzzle"></param>
    private void SetNextPuzzle(Object puzzle)
    {
        Debug.Log("setting next puzzle to " + puzzle.name);
        _nextPuzzle = puzzle;
    }

    /// <summary>
    /// Cues the demon to leave the screen.
    /// </summary>
    private void DemonLeaveScreen()
    {
        hideTimer.enabled = false;
        Demon.Instance.MoveDemonOffScreen();
    }

    /// <summary>
    /// Plays the fade to black animation when the player's hide timer runs out.
    /// </summary>
    public void TransitionToNextLevel()
    {
        _anim.Play("FightFadeToBlack");
    }
    
    /// <summary>
    /// Loads the specified scene, called by the fade to black animator.
    /// </summary>
    private void LoadScene()
    {
        SceneManager.LoadScene(_nextPuzzle.name);
    }
    
    /// <summary>
    /// Changes the scene to the game over scene.
    /// </summary>
    /// <param name="won">True if the player won, false otherwise.</param>
    private void GameOver(bool won)
    {
        SceneManager.LoadScene(won ? winScene.name : loseScene.name);
    }

    /// <summary>
    /// Unsubscribes from game events.
    /// </summary>
    private void OnDestroy()
    {
        GameEvent.OnHideTimerStart -= StartHideTimer;
        GameEvent.OnHideTimerStop -= StopHideTimer;
        GameEvent.OnNextPuzzle -= SetNextPuzzle;
        GameEvent.OnGameOver -= GameOver;
    }
}