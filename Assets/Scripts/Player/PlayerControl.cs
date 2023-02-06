using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
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
    [SerializeField] private Weapon weapon;

    /// <summary>
    /// The player's health bar.
    /// </summary>
    [SerializeField] private PlayerHealthBar healthBar;

    /// <summary>
    /// The player's max health.
    /// </summary>
    [SerializeField] static int maxHealth = 100;

    /// <summary>
    /// The player's current health.
    /// </summary>
    [SerializeField] int currentHealth = maxHealth;

    /// <summary>
    /// Property for other scripts to access currentHealth and maxHealth
    /// </summary>
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    /// <summary>
    /// Test values for player health.
    /// </summary>
    [SerializeField] int testHeal = 10;

    [SerializeField] int testDamage = -10;

    /// <summary>
    /// The player's rigidbody component.
    /// </summary>
    private Rigidbody2D _rb;

    /// <summary>
    /// The main camera.
    /// </summary>
    private Camera _camera;

    /// <summary>
    /// The direction in which the player is facing.
    /// </summary>
    private Vector2 _direction = Vector2.down;

    /// <summary>
    /// The animator attached to the player.
    /// </summary>
    private Animator _anim;

    /// <summary>
    /// The slope of the diagonal along the screen.
    /// </summary>
    private float _slope;

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
        _anim = GetComponent<Animator>();
        _camera = Camera.main;
        _slope = Screen.height / Screen.width;
    }

    /// <summary>
    /// Checks for input to move and animate the player.
    /// </summary>
    private void Update()
    {
        HandleMovement();
        // HandleMouseDirection();
        HandleShooting();
        TestHealth();
    }

    /// <summary>
    /// Takes the amount of damage/healing done as an input and changes player health.
    /// </summary>
    public void UpdateHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        healthBar.UpdateHealthBar();

        if (currentHealth == 0)
        {
            SelfDestruct();
        }
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
            _direction = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            dir.y = -1f;
            _direction = Vector2.down;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            dir.x = 1f;
            _direction = Vector2.right;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            dir.x = -1f;
            _direction = Vector2.left;
        }

        PlayAnimation(dir);
        _rb.velocity = dir.normalized * speed;
    }

    /// <summary>
    /// Testing health.
    /// </summary>
    private void TestHealth()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UpdateHealth(testHeal);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            UpdateHealth(testDamage);
        }
    }

    /// <summary>
    /// Self destructs
    /// </summary>
    private void SelfDestruct()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        weapon.GetComponent<SpriteRenderer>().enabled = false;
    }

    // todo: mercedes fix this
    /// <summary>
    /// Handles the direction in which the player is facing from mouse location.
    /// </summary>
    private void HandleMouseDirection()
    {
        Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = _camera.ScreenToWorldPoint(transform.position);
        Vector2 dir = Vector2.zero;
        
        if (mousePos.x <= playerPos.x && Math.Abs(playerPos.y) <= _slope * playerPos.x)
        {
            dir.x = -1;
            _direction = Vector2.left;
        }
        else if (mousePos.x >= playerPos.x && Math.Abs(playerPos.y) <= _slope * playerPos.x)
        {
            dir.x = 1;
            _direction = Vector2.right;
        }

        if (mousePos.y <= playerPos.y && Math.Abs(playerPos.x) <= 1 / _slope * playerPos.y)
        {
            dir.y = -1;
            _direction = Vector2.down;
        }
        else if (mousePos.y >= playerPos.y && Math.Abs(playerPos.x) <= 1 / _slope * playerPos.y)
        {
            dir.y = 1;
            _direction = Vector2.up;
        }
        
        PlayAnimation(dir);
    }

    /// <summary>
    /// Animates the player walking.
    /// </summary>
    /// <param name="direction"></param>
    private void PlayAnimation(Vector2 direction)
    {
        switch (direction.y)
        {
            case -1:
                _anim.Play("PlayerFrontWalk");
                break;
            case 1:
                _anim.Play("PlayerBackWalk");
                break;
            default:
                switch (direction.x)
                {
                    case -1:
                        _anim.Play("PlayerLeftWalk");
                        break;
                    case 1:
                        _anim.Play("PlayerRightWalk");
                        break;
                    default:
                        if (_direction == Vector2.right)
                        {
                            _anim.Play("PlayerRightIdle");
                        }
                        else if (_direction == Vector2.left)
                        {
                            _anim.Play("PlayerLeftIdle");
                        }
                        else if (_direction == Vector2.up)
                        {
                            _anim.Play("PlayerBackIdle");
                        }
                        else
                        {
                            _anim.Play("PlayerFrontIdle");
                        }

                        break;
                }

                break;
        }
    }

    /// <summary>
    /// Handles the player shooting.
    /// </summary>
    private void HandleShooting()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if (GetComponent<SpriteRenderer>().enabled == false) return;

        weapon.Shoot();
    }
}