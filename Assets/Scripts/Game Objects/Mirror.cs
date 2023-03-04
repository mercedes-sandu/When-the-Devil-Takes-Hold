using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Mirror : MonoBehaviour, IInteractable
{
    /// <summary>
    /// The endpoint of the laser.
    /// </summary>
    [SerializeField] private Transform laserEndpoint;
    
    /// <summary>
    /// Degrees to rotate the window by upon interaction.
    /// </summary>
    [SerializeField] private int degrees = 25;

    /// <summary>
    /// The box collider for the laser.
    /// </summary>
    [SerializeField] private BoxCollider2D laserCollider;

    /// <summary>
    /// (hard coded) the number of interactions required to kill the NPC.
    /// </summary>
    [SerializeField] private int numInteractionsRequired;

    /// <summary>
    /// (hard coded) the NPC to be killed.
    /// </summary>
    [SerializeField] private NPC npc;

    /// <summary>
    /// (hard coded) the current number of interactions.
    /// </summary>
    private int _numInteractions = 0;
    
    /// <summary>
    /// True when mirror has been completely turned to the right. False when completely turned to the left.
    /// </summary>
    private bool _turned = false;

    /// <summary>
    /// The laser.
    /// </summary>
    private LineRenderer _laser;

    /// <summary>
    /// The laser start position.
    /// </summary>
    private Vector3 _startPos;

    /// <summary>
    /// The laser end position.
    /// </summary>
    private Vector3 _endPos;

    /// <summary>
    /// Initializes the laser.
    /// </summary>
    private void Awake()
    {
        _laser = GetComponent<LineRenderer>();
        _laser.positionCount = 2;
        _startPos = Vector3.zero;
        _endPos = laserEndpoint.localPosition;
        _laser.SetPosition(0, _startPos);
        _laser.SetPosition(1, _endPos);
        // ModifyLaserCollider();
    }

    /// <summary>
    /// Updates the laser position.
    /// </summary>
    private void Update()
    {
        _endPos = laserEndpoint.localPosition;
        _laser.SetPosition(1, _endPos);
    }
    
    /// <summary>
    /// Calls for the UI to be updated.
    /// </summary>
    /// <param name="interactor">The interactor component for the mirror.</param>
    /// <returns>True if the interaction was successful, false otherwise.</returns>
    public bool Interact(Interactor interactor)
    {
        Transform t = transform;
        
        if (transform.rotation.y > Quaternion.Euler(new Vector3(0, 40, 0)).y)
        {
            _turned = true;
        }

        if (transform.rotation.y < Quaternion.Euler(new Vector3 (0, 50, 0)).y & !_turned)
        {
            laserEndpoint.transform.RotateAround(t.position, t.forward, degrees);
            // ModifyLaserCollider();
            transform.rotation *= Quaternion.Euler(Vector3.up * degrees);
        }
        else
        {
            if (transform.rotation.y > Quaternion.Euler(new Vector3(0, -40, 0)).y)
            {
                laserEndpoint.transform.RotateAround(t.position, t.forward, -degrees);
                // ModifyLaserCollider();
                transform.rotation *= Quaternion.Euler(Vector3.up * -degrees);
            }
            else _turned = false;
        }

        _numInteractions++;
        
        if (_numInteractions == numInteractionsRequired) npc.UpdateHealth(-npc.GetCurrentHealth());

        return true;
    }

    // /// <summary>
    // /// Modifies the laser collider.
    // /// </summary>
    // private void ModifyLaserCollider()
    // {
    //     Vector3 startPos = transform.position;
    //     Vector3 endPos = laserEndpoint.position;
    //     
    //     // set size of collider to length of line
    //     float lineLength = Vector3.Distance(startPos, endPos);
    //     laserCollider.size = new Vector3(lineLength, _laser.startWidth);
    //     
    //     // move collider to the middle of the line
    //     Vector3 midPoint = (startPos + endPos) / 2;
    //     laserCollider.transform.position = midPoint;
    //     
    //     // rotate the collider to match the rotation of the line
    //     float angle = Mathf.Abs(startPos.y - endPos.y) / Mathf.Abs(startPos.x - endPos.x);
    //     if ((startPos.y < endPos.y && startPos.x > endPos.x) ||
    //         (endPos.y < startPos.y && endPos.x > startPos.x))
    //     {
    //         angle *= -1;
    //     }
    //     angle = Mathf.Rad2Deg * Mathf.Atan(angle);
    //     laserCollider.transform.Rotate(0, 0, angle);
    // }

    // /// <summary>
    // /// Kills NPCs on impact (rip).
    // /// </summary>
    // /// <param name="col">The collider.</param>
    // private void OnTriggerEnter2D(Collider2D col)
    // {
    //     if (!col.CompareTag("NPC")) return;
    //     Debug.Log("collided with NPC");
    //     NPC npc = col.GetComponent<NPC>();
    //     npc.UpdateHealth(-npc.GetCurrentHealth());
    // }
    // /// <summary>
    // /// Kills NPCs on impact (rip).
    // /// </summary>
    // /// <param name="col">The collision.</param>
    // private void OnCollisionEnter2D(Collision2D col)
    // {
    //     if (!col.collider.CompareTag("NPC")) return;
    //     Debug.Log("collided with NPC");
    //     NPC npc = col.collider.GetComponent<NPC>();
    //     npc.UpdateHealth(-npc.GetCurrentHealth());
    // }
}