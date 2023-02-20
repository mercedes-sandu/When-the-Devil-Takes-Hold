using System.Collections;
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
    /// The fireball prefabs.
    /// </summary>
    [SerializeField] private GameObject[] fireballPrefabs;

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
    /// The time it takes the demon to move.
    /// </summary>
    [SerializeField] private float moveTime;

    /// <summary>
    /// The time the demon stays at a position before moving.
    /// </summary>
    [SerializeField] private float stayTime;

    /// <summary>
    /// The amount of damage the demon's attack deals.
    /// </summary>
    [SerializeField] private int damage;
    
    /// <summary>
    /// The demon's max health.
    /// </summary>
    [SerializeField] private static readonly int MaxHealth = 1000;

    /// <summary>
    /// The demon's health bar.
    /// </summary>
    [SerializeField] private Image healthBar;
    
    /// <summary>
    /// The health bar UI offset
    /// </summary>
    [SerializeField] private Vector3 offset;

    /// <summary>
    /// The transforms of the points the demon can move to.
    /// </summary>
    [SerializeField] private Transform[] movePoints;
    
    /// <summary>
    /// The transform of the player's interaction point.
    /// </summary>
    [SerializeField] private Transform interactionPoint;
    
    /// <summary>
    /// The radius around the interaction point that will be checked for interactable objects.
    /// </summary>
    [SerializeField] private float interactionPointRadius = 1.75f;
    
    /// <summary>
    /// The layer(s) that interactable objects are on.
    /// </summary>
    [SerializeField] private LayerMask interactableMask;
    
    /// <summary>
    /// The number of interactable objects found.
    /// </summary>
    [SerializeField] private int numFound;

    /// <summary>
    /// The list of interactable objects the player can access.
    /// </summary>
    private readonly BoxCollider2D[] _colliders = new BoxCollider2D[3];

    /// <summary>
    /// The demon's current health.
    /// </summary>
    private int _currentHealth = MaxHealth;

    /// <summary>
    /// The demon's animator component.
    /// </summary>
    private Animator _anim;

    /// <summary>
    /// The main camera.
    /// </summary>
    private Camera _camera;

    /// <summary>
    /// The cached moving boolean for the animator.
    /// </summary>
    private static readonly int Moving = Animator.StringToHash("moving");

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

        _anim = GetComponent<Animator>();
        _anim.SetBool(Moving, false);

        _camera = Camera.main;
    }

    /// <summary>
    /// Starts spawning fireballs and randomly moving the demon.
    /// </summary>
    private void Start()
    {
        healthBar.transform.position = _camera.WorldToScreenPoint(transform.position + offset);
        InvokeRepeating(nameof(FireProjectile), 5, fireTime);
        InvokeRepeating(nameof(MoveDemon), stayTime, stayTime + moveTime);
    }
    
    /// <summary>
    /// Consistently checks for the player to do a melee attack, and moves the health bar with the demon.
    /// </summary>
    private void Update()
    {
        healthBar.transform.position = _camera.WorldToScreenPoint(transform.position + offset);
        numFound = Physics2D.OverlapCircleNonAlloc(interactionPoint.position, interactionPointRadius, 
            _colliders, interactableMask);

        if (numFound <= 0) return;
        Attack();
        // StartCoroutine(AttackCooldown());
    }

    /// <summary>
    /// Draws the interaction radius in the editor.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactionPoint.position, interactionPointRadius);
    }

    /// <summary>
    /// Attacks the player.
    /// </summary>
    private void Attack()
    {
        _anim.Play("DemonAttack");
    }
    
    /// <summary>
    /// Waits a second to attack again.
    /// </summary>
    /// <returns></returns>
    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(1);
    }

    /// <summary>
    /// Updates the demon's health.
    /// </summary>
    /// <param name="damage">The amount of damage the demon took.</param>
    public void UpdateHealth(int damage)
    {
        _currentHealth += damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, MaxHealth);

        _anim.Play("DemonHurt");
        
        UpdateDemonHealthBar();

        if (_currentHealth != 0) return;
        CancelInvoke();
        _anim.Play("DemonDie");
    }

    /// <summary>
    /// Fires a projectile from the list of projectile prefabs.
    /// </summary>
    private void FireProjectile()
    {
        Vector3 pos = transform.position;
        Vector3 ballPos = new Vector3(pos.x + fireballDistance, pos.y + fireballY, pos.z);
        GameObject prefab = fireballPrefabs[Random.Range(0, fireballPrefabs.Length)];
        AIDestinationSetter setter = prefab.GetComponent<AIDestinationSetter>();
        if (setter)
        {
            GameObject fireball = Instantiate(prefab, ballPos, Quaternion.identity);
            fireball.GetComponent<AIDestinationSetter>().target = PlayerControl.Instance.gameObject.transform;
        }
        else
        {
            Vector3 playerDirection = PlayerControl.Instance.transform.position - transform.position;
            float lookAngle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;
            GameObject fireball = Instantiate(prefab, ballPos, Quaternion.Euler(0, 0, lookAngle));
        }
    }

    /// <summary>
    /// Updates the demon health bar whenever the demon takes damage or heals.
    /// </summary>
    private void UpdateDemonHealthBar()
    {
        healthBar.fillAmount = Mathf.Clamp((float) _currentHealth / MaxHealth, 0, MaxHealth);
    }

    /// <summary>
    /// Picks a random position to move the demon to.
    /// </summary>
    private void MoveDemon()
    {
        _anim.SetBool(Moving, true);
        StartCoroutine(MoveTo(moveTime, movePoints[Random.Range(0, movePoints.Length)].position));
    }

    /// <summary>
    /// Moves the demon to the specified position in the amount of time specified.
    /// </summary>
    /// <param name="time">The amount of time it takes to move the demon.</param>
    /// <param name="endPos">The end position of the movement.</param>
    /// <returns></returns>
    private IEnumerator MoveTo(float time, Vector3 endPos)
    {
        Vector3 startPos = transform.position;

        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        _anim.SetBool(Moving, false);
    }

    /// <summary>
    /// Called by the animator during the demon's attack animation.
    /// </summary>
    public void AttackPlayer()
    {
        PlayerControl.Instance.UpdateHealth(damage);
    }
    
    /// <summary>
    /// Called by the animator when the demon finishes the dying animation.
    /// </summary>
    public void Die()
    {
        GameEvent.EndGame(true);
        Destroy(gameObject);
    }
}