using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    /// <summary>
    /// The moving speed of the bullet.
    /// </summary>
    [SerializeField] private float speed;

    /// <summary>
    /// The amount of damage the bullet does.
    /// </summary>
    [SerializeField] private int damage;

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

    /// <summary>
    /// Does damage appropriately.
    /// </summary>
    /// <param name="collision">The collision with the bullet.</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Demon"))
        {
            Demon.Instance.UpdateHealth(damage);
        }
        Destroy(gameObject);
    }
}