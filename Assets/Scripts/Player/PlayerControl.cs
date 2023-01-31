using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControl : MonoBehaviour
{
    /// <summary>
    /// The instance of the player accessible by all classes.
    /// </summary>
    public static PlayerControl Instance = null;
    
    /// <summary>
    /// The player's movement speed.
    /// </summary>
    [SerializeField] private float speed = 5f;

    /// <summary>
    /// The player's projectile object (child of the player).
    /// </summary>
    [SerializeField] private Projectile projectile;

    /// <summary>
    /// The player's rigidbody component.
    /// </summary>
    private Rigidbody2D _rb;

    /// <summary>
    /// The main camera.
    /// </summary>
    private Camera _camera;

    /// <summary>
    /// Initializes components and variables.
    /// </summary>
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        
        _rb = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
    }

    /// <summary>
    /// Checks for input to move and animate the player.
    /// </summary>
    private void Update()
    {
        HandleMovement();
        HandleMouseDirection();
        HandleShooting();
    }
    
    /// <summary>
    /// Moves the player in the correct direction based on arrow keys or WASD.
    /// </summary>
    private void HandleMovement() 
    {
        Vector2 dir = Vector2.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            dir.y = 1f;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            dir.y = -1f;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            dir.x = 1f;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            dir.x = -1f;
        }

        _rb.velocity = dir.normalized * speed;
    }

    /// <summary>
    /// Handles the direction in which the player is facing from mouse location.
    /// </summary>
    private void HandleMouseDirection()
    {
        Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = _camera.ScreenToWorldPoint(transform.position);
        
        // todo: may need to change the sprite to look right
        transform.localScale = new Vector3(1, mousePos.x >= playerPos.x ? 1 : -1, 1);
        
        if (mousePos.y <= playerPos.y)
        {
            // make the player look down (sprite)
        }
        else
        {
            // make the player look up (sprite)
        }
    }

    /// <summary>
    /// Handles the player shooting.
    /// </summary>
    private void HandleShooting()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        projectile.Shoot();
    }
}