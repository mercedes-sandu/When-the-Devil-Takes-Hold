using UnityEngine;

public class NPC : MonoBehaviour
{
    /// <summary>
    /// The NPC's max health.
    /// </summary>
    [SerializeField] private static readonly int MaxHealth = 50;

    /// <summary>
    /// The NPC's current health.
    /// </summary>
    private int _currentHealth = MaxHealth;
    
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
            SelfDestruct();
        }
    }

    private void UpdateNPCSpriteColor()
    {
        
    }
    
    private void SelfDestruct()
    {
        
    }
}