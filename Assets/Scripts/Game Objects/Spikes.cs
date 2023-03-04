using System.Collections;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    /// <summary>
    /// The time it takes for the spikes to go up.
    /// </summary>
    [SerializeField] private float waitTime = 5f;

    /// <summary>
    /// The amount of damage the spikes deal.
    /// </summary>
    [SerializeField] private int damage = -5;
    
    /// <summary>
    /// The minimum x-position of the spikes.
    /// </summary>
    private float _minX;

    /// <summary>
    /// The maximum x-position of the spikes.
    /// </summary>
    private float _maxX;
    
    /// <summary>
    /// The minimum y-position of the spikes.
    /// </summary>
    private float _minY;
    
    /// <summary>
    /// The maximum y-position of the spikes.
    /// </summary>
    private float _maxY;
    
    /// <summary>
    /// True if the spikes are currently up, false otherwise.
    /// </summary>
    private bool _up = false;

    /// <summary>
    /// True if the player has been damaged by the spikes, false otherwise.
    /// </summary>
    private bool _damagedPlayer = false;

    /// <summary>
    /// The spikes animator component.
    /// </summary>
    private Animator _anim;
    
    /// <summary>
    /// The spikes audio source component.
    /// </summary>
    private AudioSource _audioSource;

    /// <summary>
    /// The spikes sprite renderer component.
    /// </summary>
    private Vector3 _boundsSize;

    /// <summary>
    /// Initialize components.
    /// </summary>
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _boundsSize = GetComponent<SpriteRenderer>().sprite.bounds.size;
    }

    private void Start()
    {
        float width = _boundsSize.x;
        float height = _boundsSize.y;
        Vector3 pos = transform.position;
        _minX = pos.x - width / 2;
        _maxX = pos.x + width / 2;
        _minY = pos.y - height / 2;
        _maxY = pos.y + height / 2;
        
        InvokeRepeating(nameof(BringUpSpikes), 1, waitTime);
    }

    /// <summary>
    /// Checks if the player is walking over the spikes. If so, damage the player.
    /// </summary>
    private void Update()
    {
        if (!_up) return;
        Vector3 playerPos = PlayerControl.Instance.transform.position;
        if (!(InRange(playerPos.x, _minX, _maxX) && InRange(playerPos.y, _minY, _maxY) && !_damagedPlayer)) return;
        _damagedPlayer = true;
        PlayerControl.Instance.UpdateHealth(damage);
        StartCoroutine(WaitToDamagePlayer());
    }

    /// <summary>
    /// Waits to damage the player.
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitToDamagePlayer()
    {
        yield return new WaitForSeconds(3);
        _damagedPlayer = false;
    }
    
    /// <summary>
    /// Brings the spikes up.
    /// </summary>
    private void BringUpSpikes()
    {
        _up = true;
        _anim.Play("SpikesRise");
        _audioSource.Play();
    }

    /// <summary>
    /// Brings the spikes down. Called by the animator.
    /// </summary>
    private void BringSpikesDown()
    {
        _up = false;
    }

    /// <summary>
    /// Returns whether the given value is within the given range (inclusive).
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <returns>True if the value is in range, false otherwise.</returns>
    private bool InRange(float value, float min, float max) => value >= min && value <= max;
}