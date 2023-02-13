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
    /// The NPC sprite renderer.
    /// </summary>
    private SpriteRenderer _sr;
    
    /// <summary>
    /// The NPC's current health.
    /// </summary>
    private int _currentHealth = MaxHealth;

    /// <summary>
    /// Initialize component.
    /// </summary>
    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
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
        Destroy(gameObject);
    }
}