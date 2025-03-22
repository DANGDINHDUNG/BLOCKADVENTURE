using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private EnemyHealth enemy;
    public int stage;

    private void Start()
    {
        RequestNewEnemy();
    }

    public void RequestNewEnemy()
    {
        enemy.CreateEnemy();
    }
}
