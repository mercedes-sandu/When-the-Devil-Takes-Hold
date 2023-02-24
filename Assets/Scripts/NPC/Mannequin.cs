using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mannequin : MonoBehaviour
{
    /// <summary>
    /// The NPC's max health.
    /// </summary>
    [SerializeField] private static readonly int MaxHealth = 10;

    /// <summary>
    /// The damage the mannequin does upon death.
    /// </summary>
    [SerializeField] private int damage = -5;

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
    /// Takes the amount of damage/healing done as an input and changes Mannequin health.
    /// </summary>
    public void UpdateHealth(int amount)
    {
        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, MaxHealth);

        UpdateNPCSpriteColor();

        if (_currentHealth == 0)
        {
            PlayerControl.Instance.UpdateHealth(damage);
            Invoke(nameof(SelfDestruct), 0.25f);
        }
    }

    /// <summary>
    /// Returns the Mannequin's current health.
    /// </summary>
    /// <returns>An integer of the NPC's current health.</returns>
    public int GetCurrentHealth() => _currentHealth;

    /// <summary>
    /// Progressively makes the Mannequin more red as they take damage (blood? who knows)
    /// </summary>
    private void UpdateNPCSpriteColor()
    {
        float ratio = (float)_currentHealth / MaxHealth;
        _sr.color = new Color(1, ratio, ratio);
    }

    /// <summary>
    /// Kills the mannequin and damages the player.
    /// </summary>
    private void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
