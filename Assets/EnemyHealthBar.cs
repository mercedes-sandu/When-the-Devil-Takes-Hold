using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;
    [SerializeField] private Demon enemy;

    public void UpdateHealthBar()
    {
        healthBarImage.fillAmount = Mathf.Clamp(enemy.currentHealth / Demon.maxHealth, 0, Demon.maxHealth);
    }
}
