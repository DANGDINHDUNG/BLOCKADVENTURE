using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : HealthPoint
{
    public void CreateEnemy()
    {
        currentHealthPoint = maxHealthPoint;
        GameEvent.UpdateEnemyHealthBar?.Invoke(currentHealthPoint, maxHealthPoint);
    }

    public override void TakeDamage(int damage)
    {
        currentHealthPoint -= damage;
        GameEvent.UpdateEnemyHealthBar?.Invoke(currentHealthPoint, maxHealthPoint);
    }
}
