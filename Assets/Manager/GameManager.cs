using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class SpawnPointData
    {
        public Transform spawnPoint;
        public float lastSpawnTime = float.NegativeInfinity;
        public float spawnTime = 10f;
    }

    [System.Serializable]
    public class EnemyPrefabData
    {
        public GameObject enemyPrefab;
        public float spawnChance = 1f;
    }

    [SerializeField] private List<SpawnPointData> spawnPoints = new List<SpawnPointData>();
    [SerializeField] private List<EnemyPrefabData> enemyPrefabs = new List<EnemyPrefabData>();

    [SerializeField] private int totalSpawns = 80;
    private int spawnsRemaining;

    private void Start()
    {
        spawnsRemaining = totalSpawns;
    }

    private void Update()
    {
        CheckRespawn();
    }

    private void CheckRespawn()
    {
        foreach (var spawnPointData in spawnPoints)
        {
            if (spawnsRemaining > 0 && Time.time >= spawnPointData.lastSpawnTime + spawnPointData.spawnTime)
            {
                if (!IsPointInView(spawnPointData.spawnPoint.position))
                {
                    SpawnEnemy(spawnPointData);
                    spawnPointData.lastSpawnTime = Time.time;
                    spawnsRemaining--;
                }
            }
        }
    }

    private bool IsPointInView(Vector3 point)
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(point);
        return viewportPosition.x >= 0 && viewportPosition.x <= 1 && viewportPosition.y >= 0 && viewportPosition.y <= 1;
    }

    private void SpawnEnemy(SpawnPointData spawnPointData)
    {
        float totalSpawnChance = 0f;
        foreach (var enemyPrefabData in enemyPrefabs)
        {
            totalSpawnChance += enemyPrefabData.spawnChance;
        }

        float randomValue = Random.Range(0f, totalSpawnChance);
        float cumulativeChance = 0f;

        foreach (var enemyPrefabData in enemyPrefabs)
        {
            cumulativeChance += enemyPrefabData.spawnChance;
            if (randomValue <= cumulativeChance)
            {
                Instantiate(enemyPrefabData.enemyPrefab, spawnPointData.spawnPoint.position, Quaternion.identity);
                break;
            }
        }
    }
}
