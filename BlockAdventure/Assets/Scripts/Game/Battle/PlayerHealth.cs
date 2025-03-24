using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : HealthPoint
{
    private void Start()
    {
        currentHealthPoint = maxHealthPoint;
        GameEvent.UpdatePlayerHealthBar?.Invoke(currentHealthPoint, maxHealthPoint);
    }

    private void OnEnable()
    {
        GameEvent.PlayerTakeDamage += TakeDamage;
    }

    private void OnDisable()
    {
        GameEvent.PlayerTakeDamage -= TakeDamage;
    }

    public override void TakeDamage(int damage)
    {
        currentHealthPoint -= damage;
        GameEvent.UpdatePlayerHealthBar?.Invoke(currentHealthPoint, maxHealthPoint);
    }
}
