using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealthPoint : MonoBehaviour
{
    public int currentHealthPoint;
    public int maxHealthPoint;

    public abstract void TakeDamage(int damage);
}
