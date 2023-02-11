using Pathfinding;
using UnityEngine;

public class Demon : MonoBehaviour
{
    public static Demon Instance = null;
    
    /// <summary>
    /// The player's health bar.
    /// </summary>
    [SerializeField] private PlayerHealthBar healthBar;
    
    public GameObject FireballPrefab;

    public float fireballDistance;
    public float fireballY;
    public float fireballVelocity;

    public float fireTime;
    
    /// <summary>
    /// The demon's max health.
    /// </summary>
    [SerializeField] static int maxHealth = 300;
    
    /// <summary>
    /// The demon's current health.
    /// </summary>
    [SerializeField] int currentHealth = maxHealth;
    
    /// <summary>
    /// Property for other scripts to access currentHealth and maxHealth
    /// </summary>
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    void Awake()
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
    
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(FireProjectile), 3, 10); // todo: change repeatrate to firetime later
    }

    void FireProjectile()
    {
        Vector3 ball_pos = new Vector3(transform.position.x + fireballDistance, transform.position.y + fireballY, transform.position.z);
        GameObject fireball = Instantiate(FireballPrefab, ball_pos, Quaternion.identity);
        //fireball.GetComponent<Rigidbody2D>().velocity = new Vector2(fireballVelocity, 0);
        fireball.GetComponent<AIDestinationSetter>().target = PlayerControl.Instance.gameObject.transform;
    }
    
    /// <summary>
    /// Takes the amount of damage/healing done as an input and changes player health.
    /// </summary>
    public void UpdateHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        healthBar.UpdateDemonHealthBar();

        if (currentHealth == 0)
        {
            SelfDestruct();
        }
    }
    
    /// <summary>
    /// Self destructs
    /// </summary>
    private void SelfDestruct()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
    }
}