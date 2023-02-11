using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Explosion : MonoBehaviour
{
    /// <summary>
    /// The explosion audio source component.
    /// </summary>
    private AudioSource _audioSource;

    /// <summary>
    /// Play the Vine thud effect.
    /// </summary>
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.Play();
    }
    
    /// <summary>
    /// Destroys the explosion instance.
    /// </summary>
    public void DestroyExplosion()
    {
        Destroy(gameObject);
    }
}