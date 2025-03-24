using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public EnemyHealth enemy;
    public PlayerHealth player;
    public static BattleManager battleInstance;
    public int stage;
    public TextMeshProUGUI _stageText;

    private void Awake()
    {
        battleInstance = this;
    }

    private void Start()
    {
        RequestNewEnemy();
    }

    public void RequestNewEnemy()
    {
        enemy.CreateEnemy();
    }

    public void SetStage()
    {
        _stageText.text = stage.ToString();
    }
}
