using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Image fillInImage;
    public TextMeshProUGUI enemyHealth;

    private void OnEnable()
    {
        GameEvent.UpdateEnemyHealthBar += UpdateHealthBar;
    }

    private void OnDisable()
    {
        GameEvent.UpdateEnemyHealthBar -= UpdateHealthBar;
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        fillInImage.fillAmount = (float)currentHealth / maxHealth;
        enemyHealth.SetText(currentHealth.ToString() + "/" + maxHealth.ToString());
    }
}
