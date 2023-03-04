using UnityEngine;

public class Hole : MonoBehaviour
{
    /// <summary>
    /// The position marker object corresponding to the position in the 'basement' that this hole leads to
    /// </summary>
    [SerializeField] private GameObject basementPositionObject;

    /// <summary>
    /// Whether or not this can transport the player or only the box
    /// </summary>
    [SerializeField] private bool usedByPlayer;

    /// <summary>
    /// Camera position of other room, for switching camera
    /// </summary>
    [SerializeField] private Vector3 roomCenter;

    /// <summary>
    /// The camera
    /// </summary>
    private Camera _camera;

    /// <summary>
    /// Sets the camera.
    /// </summary>
    void Start()
    {
        _camera = Camera.main;
    }

    /// <summary>
    /// Detects collisions.
    /// </summary>
    /// <param name="col">The collider.</param>
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("SlidingBox"))
        {
            col.transform.position = basementPositionObject.transform.position;
        }
        if (usedByPlayer && col.CompareTag("Player"))
        {
            col.transform.position = basementPositionObject.transform.position;
            _camera.transform.position = roomCenter;
        }
    }
}
