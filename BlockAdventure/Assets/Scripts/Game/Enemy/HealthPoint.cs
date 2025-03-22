using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealthPoint : MonoBehaviour
{
    public int currentHealthPoint;
    public int maxHealthPoint;

    private void OnEnable()
    {
        GameEvent.TakeDamage += TakeDamage;
    }

    private void OnDisable()
    {
        GameEvent.TakeDamage -= TakeDamage;
    }

    public abstract void TakeDamage(int damage);
}
