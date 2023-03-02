using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerControl : MonoBehaviour
{
    /// <summary>
    /// The instance of the player accessible by all classes.
    /// </summary>
    public static PlayerControl Instance = null;

    /// <summary>
    /// True if the player can move, false otherwise.
    /// </summary>
    public bool CanMove = true;

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
    [SerializeField] private Image healthBar;
    
    /// <summary>
    /// The flash time for the player to turn red when taking damage.
    /// </summary>
    [SerializeField] private float flashTime = 0.1f;

    /// <summary>
    /// The explosion prefab.
    /// </summary>
    [SerializeField] private GameObject explosionPrefab;

    /// <summary>
    /// The explosion animation clip.
    /// </summary>
    [SerializeField] private AnimationClip explosionAnimation;

    /// <summary>
    /// The player's max health.
    /// </summary>
    [SerializeField] private static readonly int MaxHealth = 100;

    /// <summary>
    /// The initial amount of bullets the player receives.
    /// </summary>
    [SerializeField] private static readonly int InitialBullets = 50;

    /// <summary>
    /// True if the player can use the weapon, false otherwise.
    /// </summary>
    [SerializeField] private bool canUseWeapon = false;

    /// <summary>
    /// The player's current health.
    /// </summary>
    private int _currentHealth = MaxHealth;

    /// <summary>
    /// The current number of bullets the player has.
    /// </summary>
    private int _currentBullets = InitialBullets;

    /// <summary>
    /// The player's rigidbody component.
    /// </summary>
    private Rigidbody2D _rb;

    /// <summary>
    /// The main camera.
    /// </summary>
    private Camera _camera;

    /// <summary>
    /// The audio source component.
    /// </summary>
    private AudioSource _audioSource;

    /// <summary>
    /// The direction in which the player is facing.
    /// </summary>
    private Vector2 _direction = Vector2.down;

    /// <summary>
    /// The animator attached to the player.
    /// </summary>
    private Animator _anim;

    /// <summary>
    /// The player's sprite renderer component.
    /// </summary>
    private SpriteRenderer _sr;

    /// <summary>
    /// The player's collider component.
    /// </summary>
    private BoxCollider2D _collider;

    /// <summary>
    /// The slope of the diagonal along the screen.
    /// </summary>
    private float _slope;

    /// <summary>
    /// The normal color of the player sprite.
    /// </summary>
    private Color _originalColor = Color.white;
    
    /// <summary>
    /// The damaged color of the player sprite.
    /// </summary>
    private Color _damagedColor = Color.red;

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
        _sr = GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
        _audioSource = GetComponent<AudioSource>();
        _camera = Camera.main;
        _slope = Screen.height / Screen.width;
    }

    /// <summary>
    /// Enables/disables the player's weapon.
    /// </summary>
    private void Start()
    {
        weapon.enabled = canUseWeapon;
        weapon.GetComponent<SpriteRenderer>().enabled = canUseWeapon;
    }

    /// <summary>
    /// Checks for input to move and animate the player.
    /// </summary>
    private void Update()
    {
        if (CanMove) HandleMovement();
        // HandleMouseDirection();
        if (canUseWeapon) HandleShooting();
    }

    /// <summary>
    /// Takes the amount of damage/healing done as an input and changes player health.
    /// </summary>
    public void UpdateHealth(int amount)
    {
        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, MaxHealth);

        UpdatePlayerHealthBar();
        FlashRed();
        _audioSource.Play();

        if (_currentHealth == 0)
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
    /// Self destructs
    /// </summary>
    private void SelfDestruct()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        weapon.GetComponent<SpriteRenderer>().enabled = false;
        Invoke(nameof(Die), explosionAnimation.length);
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

    /// <summary>
    /// Updates the player health bar whenever the player takes damage or heals.
    /// </summary>
    private void UpdatePlayerHealthBar()
    {
        healthBar.fillAmount = Mathf.Clamp((float) _currentHealth / MaxHealth, 0, MaxHealth);
    }

    /// <summary>
    /// returns the current rigidbody velocity of the player
    /// </summary>
    public Vector3 GetVelocity()
    {
        return _rb.velocity;
    }
    
    /// <summary>
    /// Makes the player flash red when taking damage.
    /// </summary>
    private void FlashRed()
    {
        _sr.color = _damagedColor;
        Invoke(nameof(ResetColor), flashTime);
    }
    
    /// <summary>
    /// Resets the color of the player's sprite.
    /// </summary>
    private void ResetColor()
    {
        _sr.color = _originalColor;
    }

    /// <summary>
    /// Hides the player by disabling the sprite renderer and collider.
    /// </summary>
    public void Hide()
    {
        _sr.enabled = false;
        _collider.enabled = false;
        canUseWeapon = false;
        weapon.GetComponent<SpriteRenderer>().enabled = false;
    }

    /// <summary>
    /// Shows the player by enabling the sprite renderer and collider.
    /// </summary>
    public void Show()
    {
        _sr.enabled = true;
        _collider.enabled = true;
        canUseWeapon = true;
        weapon.GetComponent<SpriteRenderer>().enabled = true;
    }

    /// <summary>
    /// Returns whether the player can use their weapon.
    /// </summary>
    /// <returns>True if the player can use their weapon, false otherwise.</returns>
    public bool CanUseWeapon() => canUseWeapon;

    /// <summary>
    /// Ends the game.
    /// </summary>
    private void Die()
    {
        GameEvent.EndGame(false);
    }
}