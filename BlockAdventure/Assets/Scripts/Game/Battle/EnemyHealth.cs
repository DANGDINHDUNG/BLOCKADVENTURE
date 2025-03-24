using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyHealth : HealthPoint
{
    public SquareTextureData squareTextureData;
    [SerializeField] private TextMeshProUGUI _turnText;

    public int actionTurn;      // Số turn tới lượt quái hành động
    public int currentTurn;     // Số turn hiện tại

    private void OnEnable()
    {
        GameEvent.EnemyTakeDamage += TakeDamage;
        GameEvent.OnEnemyTurn += EnemyTurn;
    }

    private void OnDisable()
    {
        GameEvent.EnemyTakeDamage -= TakeDamage;
        GameEvent.OnEnemyTurn -= EnemyTurn;
    }

    public void CreateEnemy()
    {
        currentHealthPoint = maxHealthPoint;
        GameEvent.UpdateEnemyHealthBar?.Invoke(currentHealthPoint, maxHealthPoint);
        currentTurn = actionTurn;
    }

    // Kiểm tra nếu tới turn hành động của quái, gây sát thương cho người chơi
    private void EnemyTurn()
    {
        currentTurn--;

        if (currentTurn == 0)
        {
            GameEvent.PlayerTakeDamage(10);
            GameEvent.CheckIfPlayerDefeated();
            currentTurn = actionTurn;
        }

        _turnText.text = currentTurn.ToString();
    }

    public override void TakeDamage(int damage)
    {
        currentHealthPoint -= damage;
        UpdateSquareColor();
        GameEvent.UpdateEnemyHealthBar?.Invoke(currentHealthPoint, maxHealthPoint);
    }

    public void UpdateSquareColor()
    {
        if (GameEvent.UpdateSquareColor != null && maxHealthPoint - currentHealthPoint >= squareTextureData.threshHoldVal)
        {
            squareTextureData.UpdateColor(maxHealthPoint - currentHealthPoint);
            GameEvent.UpdateSquareColor(squareTextureData.currentColor);
        }
    }
}
