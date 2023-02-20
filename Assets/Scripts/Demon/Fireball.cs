using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    /// <summary>
    /// The damage this fireball causes.
    /// </summary>
    [SerializeField] private int damage;
    
    /// <summary>
    /// The amount of time for which the fireball will be alive.
    /// </summary>
    [SerializeField] private float lifetime = 10;

    /// <summary>
    /// The speed of the fireball.
    /// </summary>
    [SerializeField] private float speed = 15;
    
    /// <summary>
    /// The AI destination setter of the fireball.
    /// </summary>
    private AIDestinationSetter _setter;

    /// <summary>
    /// The rigidbody of the fireball.
    /// </summary>
    private Rigidbody2D _rb;

    /// <summary>
    /// Starts the fireball's existence.
    /// </summary>
    void Start()
    {
        _setter = GetComponent<AIDestinationSetter>();
        _rb = GetComponent<Rigidbody2D>();
        if (!_setter)
        {
            _rb.velocity = transform.right * speed;
        }
        StartCoroutine(Existence());
    }

    /// <summary>
    /// Destroys the fireball after its lifetime is up.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Existence()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    /// <summary>
    /// Does damage appropriately.
    /// </summary>
    /// <param name="collision">The collision with the fireball.</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Bullet") || collision.collider.CompareTag("Demon")) return;
        if (collision.collider.CompareTag("Player"))
        {
            PlayerControl.Instance.UpdateHealth(damage);
        }
        Destroy(gameObject);
    }
}