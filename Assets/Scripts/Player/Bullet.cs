using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    /// <summary>
    /// The moving speed of the bullet.
    /// </summary>
    [SerializeField] private float speed;

    /// <summary>
    /// The bullet's rigidbody component.
    /// </summary>
    [SerializeField] private Rigidbody2D rb;

    /// <summary>
    /// Starts the bullet's existence.
    /// </summary>
    private void Start()
    {
        rb.velocity = transform.right * speed;
    }

    /// <summary>
    /// Destroys the bullet after it goes off screen.
    /// </summary>
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}