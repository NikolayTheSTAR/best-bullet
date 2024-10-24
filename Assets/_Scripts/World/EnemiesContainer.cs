using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TheSTAR.Data;
using TheSTAR.Utility;
using UnityEngine;
using Zenject;

public class EnemiesContainer : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;

    private List<Enemy> activeEnemies = new();

    private readonly ResourceHelper<GameConfig> gameConfig = new("Configs/GameConfig");

    private DataController data;
    private BulletsContainer bullets;

    private const float SpawnStep = 5;
    private const int EnemiesLimit = 5;

    [Inject]
    private void Construct(DataController data, BulletsContainer bullets)
    {
        this.data = data;
        this.bullets = bullets;
    }

    private void Start()
    {
        WaitForSpawn(SpawnStep);
    }

    private void WaitForSpawn(float timeWait)
    {
        DOVirtual.Float(0f, 1f, timeWait, (value) => {}).OnComplete(() =>
        {
            if (activeEnemies.Count < EnemiesLimit) SpawnRandomEnemy();
            WaitForSpawn(SpawnStep);
        }).SetEase(Ease.Linear);
    }

    [ContextMenu("SpawnRandomEnemy")]
    private void SpawnRandomEnemy()
    {
        var randomPos = ArrayUtility.GetRandomValue(spawnPoints);
        var enemy = Instantiate(enemyPrefab, randomPos.position, Quaternion.identity, transform);
        enemy.Init(bullets, gameConfig.Get.EnemyMaxHP, gameConfig.Get.EnemyMaxHP);
        activeEnemies.Add(enemy);
    }
}