using UnityEngine;

public class NPC : MonoBehaviour
{
    /// <summary>
    /// The NPC's max health.
    /// </summary>
    [SerializeField] private static readonly int MaxHealth = 50;

    /// <summary>
    /// The amount of time that will increase for the kill timer when the NPC is killed.
    /// </summary>
    [SerializeField] private int killTimerModifier = 10;

    /// <summary>
    /// The explosion prefab.
    /// </summary>
    [SerializeField] private GameObject explosionPrefab;

    /// <summary>
    /// The player's movement speed.
    /// </summary>
    [SerializeField] private float speed = 5f;

    /// <summary>
    /// The player's rigidbody component.
    /// </summary>
    private Rigidbody2D _rb;

    private BoxCollider2D _collider;

    /// <summary>
    /// The point at which the NPC stops walking
    /// </summary>
    private Vector3 _stoppingPoint;

    private string dir;

    private bool moving;

    /// <summary>
    /// The NPC sprite renderer.
    /// </summary>
    private SpriteRenderer _sr;
    
    /// <summary>
    /// The NPC's current health.
    /// </summary>
    private int _currentHealth = MaxHealth;

    /// <summary>
    /// The direction in which the player is facing.
    /// </summary>
    private Vector2 _direction = Vector2.down;

    /// <summary>
    /// Initialize component.
    /// </summary>
    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        moving = false;
        dir = "Down";
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (moving)
        {
            if (dir == "Down")
            {
                if (_rb.position.y <= _stoppingPoint.x)
                {
                    StopWalk();
                }
            }
            else if (dir == "Up")
            {
                if (_rb.position.y >= _stoppingPoint.y)
                {
                    StopWalk();
                }
            }
            else if (dir == "Left")
            {
                if (_rb.position.x <= _stoppingPoint.x)
                {
                    StopWalk();
                }
            }
            else if (dir == "Right")
            {
                if (_rb.position.x >= _stoppingPoint.x)
                {
                    StopWalk();
                }
            }
        }
    }

    /// <summary>
    /// Takes the amount of damage/healing done as an input and changes player health.
    /// </summary>
    public void UpdateHealth(int amount)
    {
        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, MaxHealth);

        UpdateNPCSpriteColor();

        if (_currentHealth == 0)
        {
            _collider.enabled = false;
            Invoke(nameof(SelfDestruct), 0.25f);
        }
    }

    /// <summary>
    /// Returns the NPC's current health.
    /// </summary>
    /// <returns>An integer of the NPC's current health.</returns>
    public int GetCurrentHealth() => _currentHealth;

    /// <summary>
    /// Progressively makes the NPC more red as they take damage (blood? who knows)
    /// </summary>
    private void UpdateNPCSpriteColor()
    {
        float ratio = (float) _currentHealth / MaxHealth;
        _sr.color = new Color(1, ratio, ratio);
    }
    
    /// <summary>
    /// Spawn explosion and kill NPC (rip). Also, increase the kill timer duration.
    /// </summary>
    private void SelfDestruct()
    {
        GameEvent.ChangeKillTimerDuration(killTimerModifier);
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        _sr.enabled = false;
        PuzzleMaster.Instance.KillNPC(this);
    }

    public void Walk(Vector3 stoppingPoint)
    {
        _stoppingPoint = stoppingPoint;
        _rb.velocity = _direction * speed;
        moving = true;
    }

    private void StopWalk()
    {
        _rb.velocity = Vector3.zero;
        moving = false;
    }

    public string GetDirection()
    {
        return dir;
    }

    public Vector3 GetDirectionVector()
    {
        return _direction;
    }

    public Vector3 GetPosition()
    {
        return _rb.position;
    }

    public void Turn(string direction)
    {
        if (direction == "Up")
        {
            _direction = Vector3.up;
        }
        else if (direction == "Down")
        {
            _direction = Vector3.down;
        }
        else if (direction == "Left")
        {
            _direction = Vector3.left;
        }
        else if (direction == "Right")
        {
            _direction = Vector3.right;
        }
    }
}