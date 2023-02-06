using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public int damage;
    public float lifetime = 10;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Existence());
    }

    private IEnumerator Existence()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Bullet")) return;
        if (collision.collider.CompareTag("Player"))
        {
            PlayerControl.Instance.UpdateHealth(damage);
        }
        Destroy(gameObject);
    }
}