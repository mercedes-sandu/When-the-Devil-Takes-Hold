using UnityEngine;

public class Pot : MonoBehaviour
{
    /// <summary>
    /// The amount of ammo stored in the pot.
    /// </summary>
    [SerializeField] private int ammoAmount = 5;
    
    /// <summary>
    /// The collider for the pot.
    /// </summary>
    private PolygonCollider2D _collider;

    /// <summary>
    /// The animator for the pot.
    /// </summary>
    private Animator _anim;
    
    /// <summary>
    /// The audio source for the pot.
    /// </summary>
    private AudioSource _audioSource;

    /// <summary>
    /// Initialize components.
    /// </summary>
    private void Awake()
    {
        _collider = GetComponent<PolygonCollider2D>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Breaks on collision with a bullet.
    /// </summary>
    /// <param name="col">The collision.</param>
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.collider.CompareTag("Bullet")) return;
        _audioSource.Play();
        _collider.enabled = false;
        _anim.Play("PotBreak");
        GameEvent.GainAmmo(ammoAmount);
    }
}