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
    private void Construct(DataController data, BulletsContainer bullets, AutoSave autoSave)
    {
        this.data = data;
        this.bullets = bullets;
        autoSave.BeforeAutoSaveGameEvent += () =>
        {
            var enemiesData = data.gameData.levelData.enemies;
            
            enemiesData.Clear();

            for (int i = 0; i < activeEnemies.Count; i++)
            {
                var activeEnemy = activeEnemies[i];
                enemiesData.Add(new DataController.EnemyData(activeEnemy.HpSystem.CurrentHP, activeEnemy.HpSystem.MaxHP, activeEnemy.transform.position));
            }
        };
    }

    private void Start()
    {
        LoadEnemies();
        WaitForSpawn(SpawnStep);
    }

    private void LoadEnemies()
    {
        var enemiesData = data.gameData.levelData.enemies;
            
        for (int i = 0; i < enemiesData.Count; i++)
        {
            var enemyData = enemiesData[i];
            SpawnEnemy(enemyData.currentHP, enemyData.maxHP, enemyData.position);
        }
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
        SpawnEnemy(gameConfig.Get.EnemyMaxHP, gameConfig.Get.EnemyMaxHP, randomPos.position);
    }

    private void SpawnEnemy(int currentHp, int maxHp, Vector3 pos)
    {
        var enemy = Instantiate(enemyPrefab, pos, Quaternion.identity, transform);
        enemy.Init(bullets, currentHp, maxHp);
        activeEnemies.Add(enemy);
    }
}