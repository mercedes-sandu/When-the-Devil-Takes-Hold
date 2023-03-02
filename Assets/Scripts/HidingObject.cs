using UnityEngine;

public class HidingObject : MonoBehaviour, IInteractable
{
    /// <summary>
    /// The amount of time the player needs to hide to escape the demon.
    /// </summary>
    [SerializeField] private int hideTime = 5;
    
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
    /// The cached hiding boolean for the animator.
    /// </summary>
    private static readonly int Hiding = Animator.StringToHash("hiding");

    /// <summary>
    /// Initializes components.
    /// </summary>
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
        _anim.SetBool(Hiding, false);
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
            GameEvent.StopHideTimer();
        }
        else
        {
            PlayerControl.Instance.CanMove = false;
            PlayerControl.Instance.Hide();
            GameEvent.StartHideTimer(hideTime);
        }

        _hiding = !_hiding;
        _anim.SetBool(Hiding, _hiding);
        _audioSource.Play();
        
        return true;
    }
}