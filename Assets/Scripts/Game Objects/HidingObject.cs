using System.Collections;
using TMPro;
using UnityEngine;

public class HidingObject : MonoBehaviour, IInteractable
{
    /// <summary>
    /// The amount of time the player needs to hide to escape the demon.
    /// </summary>
    [SerializeField] private int hideTime = 5;

    /// <summary>
    /// The amount of time added (if positive) or taken away (if negative) from the kill timer when the player
    /// successfully hides.
    /// </summary>
    [SerializeField] private int killTimerModifier = -5;
    
    /// <summary>
    /// The hide timer text object.
    /// </summary>
    [SerializeField] private TextMeshProUGUI hideTimer;
    
    /// <summary>
    /// The fade to black animator.
    /// </summary>
    [SerializeField] private Animator fadeToBlackAnim;
    
    /// <summary>
    /// The audio source of this object.
    /// </summary>
    private AudioSource _audioSource;
    
    /// <summary>
    /// The animator of this object.
    /// </summary>
    private Animator _anim;

    /// <summary>
    /// True if the player is currently hiding, false otherwise.
    /// </summary>
    private bool _hiding = false;
    
    /// <summary>
    /// The amount of seconds left on the kill timer.
    /// </summary>
    private int _secondsLeft;
    
    /// <summary>
    /// The last coroutine which occurred.
    /// </summary>
    private Coroutine _lastCoroutine = null;

    /// <summary>
    /// True if a coroutine is running, false otherwise.
    /// </summary>
    private bool _coroutineRunning = false;

    /// <summary>
    /// The cached hiding boolean for the animator.
    /// </summary>
    private static readonly int Hiding = Animator.StringToHash("hiding");

    /// <summary>
    /// Initializes and subscribes to game events.
    /// </summary>
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
        _anim.SetBool(Hiding, false);

        GameEvent.OnDemonLeave += TransitionToNextLevel;
    }

    /// <summary>
    /// Disables the hide timer.
    /// </summary>
    private void Start()
    {
        hideTimer.enabled = false;
    }
    
    /// <summary>
    /// Hides/shows the player behind/in this object.
    /// </summary>
    /// <param name="interactor">The interactor (the player).</param>
    /// <returns></returns>
    public bool Interact(Interactor interactor)
    {
        if (_hiding)
        {
            PlayerControl.Instance.CanMove = true;
            PlayerControl.Instance.Show();
            StopHideTimer();
        }
        else
        {
            PlayerControl.Instance.CanMove = false;
            PlayerControl.Instance.Hide();
            StartHideTimer(hideTime);
        }

        _hiding = !_hiding;
        _anim.SetBool(Hiding, _hiding);
        _audioSource.Play();
        
        return true;
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
    private void TransitionToNextLevel()
    {
        if (!_hiding) return;
        MainManager.Instance.killTimerModifier = killTimerModifier;
        fadeToBlackAnim.Play("FightFadeToBlack");
    }

    /// <summary>
    /// Unsubscribes from game events.
    /// </summary>
    private void OnDestroy()
    {
        GameEvent.OnDemonLeave -= TransitionToNextLevel;
    }
}