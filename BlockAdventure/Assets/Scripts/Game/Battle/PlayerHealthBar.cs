using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Image fillInImage;
    public TextMeshProUGUI playerHealth;

    private void OnEnable()
    {
        GameEvent.UpdatePlayerHealthBar += UpdateHealthBar;
    }

    private void OnDisable()
    {
        GameEvent.UpdatePlayerHealthBar -= UpdateHealthBar;
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        fillInImage.fillAmount = (float)currentHealth / maxHealth;
        playerHealth.SetText(currentHealth.ToString() + "/" + maxHealth.ToString());
    }
}
