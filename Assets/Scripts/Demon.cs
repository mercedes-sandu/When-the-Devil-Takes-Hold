using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Demon : MonoBehaviour
{ 
    public static Demon Instance = null;

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

    [SerializeField] public static int maxHealth = 100;
    [SerializeField] public int currentHealth = maxHealth;
    [SerializeField] private EnemyHealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {

    }

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

    // Update is called once per frame
    void Update()
    {
    
    }

    private void FixedUpdate()
    {
        if (timeTick > fireTime)
        {
            FireProjectile();
            timeTick = 0;
        }
        else
        {
            timeTick++;
        }
    }

    public void UpdateHealth(int damage)
    {
        currentHealth += damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        healthBar.UpdateHealthBar();

        if (currentHealth == 0)
        {
            Destroy(gameObject);
        }
    }

    void FireProjectile()
    {
        Vector3 ball_pos = new Vector3(transform.position.x + fireballDistance, transform.position.y + fireballY, transform.position.z);
        GameObject fireball = Instantiate(FireballPrefab, ball_pos, Quaternion.identity);
        fireball.GetComponent<AIDestinationSetter>().target = PlayerControl.Instance.gameObject.transform;
    }

}
