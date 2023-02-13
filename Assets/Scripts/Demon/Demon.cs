using Pathfinding;
using UnityEngine;
using UnityEngine.UI;

public class Demon : MonoBehaviour
{ 
    /// <summary>
    /// The instance of the demon, accessible by all other classes.
    /// </summary>
    public static Demon Instance = null;

    /// <summary>
    /// The fireball prefab.
    /// </summary>
    [SerializeField] private GameObject fireballPrefab;

    /// <summary>
    /// The offset at which the fireball is spawned.
    /// </summary>
    [SerializeField] private float fireballDistance;
    
    /// <summary>
    /// The y-position at which the fireball is spawned.
    /// </summary>
    [SerializeField] private float fireballY;

    /// <summary>
    /// The time it takes to spawn a fireball.
    /// </summary>
    [SerializeField] private float fireTime;
    
    /// <summary>
    /// The demon's max health.
    /// </summary>
    [SerializeField] private static readonly int MaxHealth = 300;

    /// <summary>
    /// The demon's health bar.
    /// </summary>
    [SerializeField] private Image healthBar;
    
    /// <summary>
    /// The demon's current health.
    /// </summary>
    private int _currentHealth = MaxHealth;

    /// <summary>
    /// Initializes the instance.
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
    }

    /// <summary>
    /// Starts spawning fireballs.
    /// </summary>
    private void Start()
    {
        InvokeRepeating(nameof(FireProjectile), 5, fireTime);
    }

    /// <summary>
    /// Updates the demon's health.
    /// </summary>
    /// <param name="damage">The amount of damage the demon took.</param>
    public void UpdateHealth(int damage)
    {
        _currentHealth += damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, MaxHealth);

        UpdateDemonHealthBar();

        if (_currentHealth != 0) return;
        CancelInvoke();
        GameEvent.EndGame(true);
        Destroy(gameObject);
    }

    /// <summary>
    /// Fires a projectile that will follow the player.
    /// </summary>
    private void FireProjectile()
    {
        Vector3 pos = transform.position;
        Vector3 ballPos = new Vector3(pos.x + fireballDistance, pos.y + fireballY, pos.z);
        GameObject fireball = Instantiate(fireballPrefab, ballPos, Quaternion.identity);
        fireball.GetComponent<AIDestinationSetter>().target = PlayerControl.Instance.gameObject.transform;
    }

    /// <summary>
    /// Updates the demon health bar whenever the demon takes damage or heals.
    /// </summary>
    private void UpdateDemonHealthBar()
    {
        healthBar.fillAmount = Mathf.Clamp((float) _currentHealth / MaxHealth, 0, MaxHealth);
    }
}