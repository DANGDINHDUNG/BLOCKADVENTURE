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
        GameEvent.UpdateEnemyHealthBar += UpdateEnemyHealthBar;
    }

    private void OnDisable()
    {
        GameEvent.UpdateEnemyHealthBar -= UpdateEnemyHealthBar;
    }

    private void UpdateEnemyHealthBar(int currentHealth, int maxHealth)
    {
        fillInImage.fillAmount = (float)currentHealth / maxHealth;
        enemyHealth.SetText(currentHealth.ToString() + "/" + maxHealth.ToString());
    }
}
