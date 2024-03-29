using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerControl : MonoBehaviour
{
    /// <summary>
    /// The instance of the player accessible by all classes.
    /// </summary>
    public static PlayerControl Instance = null;

    public bool hasKey;

    /// <summary>
    /// True if the player can move, false otherwise.
    /// </summary>
    public bool CanMove = true;

    /// <summary>
    /// True if the player can detonate TNT, false otherwise.
    /// </summary>
    public bool CanDetonateTNT = true;

    /// <summary>
    /// The player's movement speed.
    /// </summary>
    [SerializeField] private float speed = 5f;

    /// <summary>
    /// The player's projectile object (child of the player).
    /// </summary>
    [SerializeField] private Weapon weapon;

    /// <summary>
    /// The player's torch.
    /// </summary>
    [SerializeField] private SpriteRenderer torch;

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
    /// True if the player can use the weapon, false otherwise.
    /// </summary>
    [SerializeField] private bool canUseWeapon = false;

    /// <summary>
    /// The player's current health.
    /// </summary>
    private int _currentHealth = MaxHealth;

    /// <summary>
    /// The player's rigidbody component.
    /// </summary>
    private Rigidbody2D _rb;

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
    /// The normal color of the player sprite.
    /// </summary>
    private readonly Color _originalColor = Color.white;
    
    /// <summary>
    /// The damaged color of the player sprite.
    /// </summary>
    private readonly Color _damagedColor = Color.red;

    /// <summary>
    /// Initializes components and variables, and subscribes to game events.
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

        hasKey = false;

        GameEvent.OnPlayerDetonate += DisableTorch;
    }

    /// <summary>
    /// Enables/disables the player's weapon.
    /// </summary>
    private void Start()
    {
        _currentHealth = MainManager.Instance.health;
        UpdatePlayerHealthBar();
        weapon.enabled = canUseWeapon;
        weapon.GetComponent<SpriteRenderer>().enabled = canUseWeapon;
    }

    /// <summary>
    /// Checks for input to move and animate the player.
    /// </summary>
    private void Update()
    {
        if (CanMove)
        {
            HandleMovement();
        }
        else
        {
            _rb.velocity = Vector2.zero;
        }
        if (canUseWeapon) HandleShooting();
    }

    /// <summary>
    /// Returns the player's weapon.
    /// </summary>
    /// <returns>The weapon the player has.</returns>
    public Weapon GetWeapon() => weapon;

    /// <summary>
    /// Sets the player's health to the specified amount. Called at the start of a new scene.
    /// </summary>
    /// <param name="amount">The amount of health the player will have.</param>
    public void SetHealth(int amount)
    {
        _currentHealth = amount;
        UpdatePlayerHealthBar();
    }
    
    /// <summary>
    /// Takes the amount of damage/healing done as an input and changes player health.
    /// </summary>
    public void UpdateHealth(int amount)
    {
        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, MaxHealth);

        MainManager.Instance.health = _currentHealth;

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

    public Vector3 GetPosition()
    {
        return _rb.position;
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
    /// Disables the player's torch.
    /// </summary>
    private void DisableTorch()
    {
        if (!torch) return;
        torch.enabled = false;
    }

    /// <summary>
    /// Ends the game.
    /// </summary>
    private void Die()
    {
        GameEvent.EndGame(false);
    }

    /// <summary>
    /// Unsubscribes from game events.
    /// </summary>
    private void OnDestroy()
    {
        GameEvent.OnPlayerDetonate -= DisableTorch;
    }
}