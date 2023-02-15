using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingBox : MonoBehaviour, IInteractable
{

    /// <summary>
    /// Indicates whether the boulder is currently being pulled or not
    /// </summary>
    public bool isPull;

    /// <summary>
    /// Rigidbody component of this boulder
    /// </summary>
    Rigidbody2D _rb;

    /// <summary>
    /// Player that interacts with the boulder
    /// </summary>
    [SerializeField] private GameObject playerObject;

    // Start is called before the first frame update

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        isPull = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPull)
        {
            //_rb.velocity = playerObject.transform.velocity;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
        {
            collision.gameObject.GetComponent<NPC>().UpdateHealth(-50);
        }
    }

    public bool Interact(Interactor interactor)
    {
        isPull = !isPull;
        return true;
    }
}
