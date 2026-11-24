using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyController enemyPrefab;
    [SerializeField] private CoreHealth core;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 5f;

    private readonly List<EnemyController> activeEnemies = new List<EnemyController>();

    private void Start()
    {
        if (enemyPrefab == null || core == null ||
            spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError(
                "Assign an enemy prefab, the Core, and at least one spawn point.",
                this
            );
            enabled = false;
            return;
        }

        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        int index = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[index];

        EnemyController enemy = Instantiate(
            enemyPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        enemy.SetCoreTarget(core);
        activeEnemies.Add(enemy);
        
    }

    public EnemyController GetNearestEnemy(Vector3 fromPosition)
    {
        activeEnemies.RemoveAll(enemy => enemy == null);

        EnemyController nearestEnemy = null;
        float nearestDistanceSquared = float.PositiveInfinity;

        foreach (EnemyController enemy in activeEnemies)
        {
            float distanceSquared =
                (enemy.transform.position - fromPosition).sqrMagnitude;

            if (distanceSquared < nearestDistanceSquared)
            {
                nearestDistanceSquared = distanceSquared;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }
}