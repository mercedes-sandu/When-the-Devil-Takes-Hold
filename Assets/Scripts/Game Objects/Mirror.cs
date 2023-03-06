using System.Collections;
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
    /// The distance the laser travels.
    /// </summary>
    [SerializeField] private float distance = 15;

    /// <summary>
    /// The damage the laser does to the player.
    /// </summary>
    [SerializeField] private int damageToPlayer = -10;

    /// <summary>
    /// The damage the laser does to the NPC.
    /// </summary>
    [SerializeField] private int damageToNPC = -50;

    /// <summary>
    /// The amount of time the player is invulnerable after being hit by the laser.
    /// </summary>
    [SerializeField] private float invulnerableTime = 1;

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
    /// True if the player is invulnerable, false otherwise.
    /// </summary>
    private bool _invulnerable = false;

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
    }

    /// <summary>
    /// Updates the laser position.
    /// </summary>
    private void Update()
    {
        _endPos = laserEndpoint.localPosition;
        _laser.SetPosition(1, _endPos);
        DetectCollisions();
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

        if (transform.rotation.y < Quaternion.Euler(new Vector3(0, 50, 0)).y & !_turned)
        {
            laserEndpoint.transform.RotateAround(t.position, t.forward, degrees);
            transform.rotation *= Quaternion.Euler(Vector3.up * degrees);
        }
        else
        {
            if (transform.rotation.y > Quaternion.Euler(new Vector3(0, -40, 0)).y)
            {
                laserEndpoint.transform.RotateAround(t.position, t.forward, -degrees);
                transform.rotation *= Quaternion.Euler(Vector3.up * -degrees);
            }
            else _turned = false;
        }

        return true;
    }

    /// <summary>
    /// Detects collisions with the laser and applies damage to the player and NPCs.
    /// </summary>
    private void DetectCollisions()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, 
            laserEndpoint.transform.position - transform.position, distance);
        foreach (RaycastHit2D hit in hits)
        {
            Debug.DrawRay(transform.position, hit.collider.transform.position - transform.position, Color.green);
            if (hit.collider.CompareTag("NPC"))
            {
                Debug.Log("hit npc " + hit.collider.name + " with mirror " + name);
                hit.collider.GetComponent<NPC>().UpdateHealth(damageToNPC);
            }
            if (hit.collider.CompareTag("Player"))
            {
                if (_invulnerable) return;
                PlayerControl.Instance.UpdateHealth(damageToPlayer);
                StartCoroutine(Cooldown());
            }
        }
    }

    /// <summary>
    /// Counts down the invulnerability time.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Cooldown()
    {
        _invulnerable = true;
        yield return new WaitForSeconds(invulnerableTime);
        _invulnerable = false;
    }
}