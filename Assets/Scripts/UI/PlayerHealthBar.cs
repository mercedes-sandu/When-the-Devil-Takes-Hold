using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;
    [SerializeField] private PlayerControl player;

    public void UpdateHealthBar()
    {
        healthBarImage.fillAmount = Mathf.Clamp(player.CurrentHealth / player.MaxHealth, 0, player.MaxHealth);
    }
}
