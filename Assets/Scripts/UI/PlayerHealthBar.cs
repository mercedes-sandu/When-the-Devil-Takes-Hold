using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;
    [SerializeField] private PlayerControl player;
    [SerializeField] private Demon demon;

    public void UpdatePlayerHealthBar()
    {
        healthBarImage.fillAmount = Mathf.Clamp(player.CurrentHealth / player.MaxHealth, 0, player.MaxHealth);
    }

    public void UpdateDemonHealthBar()
    {
        healthBarImage.fillAmount = Mathf.Clamp(demon.CurrentHealth / demon.MaxHealth, 0, demon.MaxHealth);
    }
}
