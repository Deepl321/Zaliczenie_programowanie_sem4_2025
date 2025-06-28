using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>();
    public int currWave;
    private int waveValue;
    public List<GameObject> enemiesToSpawn = new List<GameObject>();

    public float spawnRadius = 30f;
    public LayerMask groundLayer;

    public int waveDuration;
    private float waveTimer;
    private float spawnInterval;
    private float spawnTimer;

    public List<GameObject> spawnedEnemies = new List<GameObject>();

    void Start()
    {
        GenerateWave();
    }

    void FixedUpdate()
    {
        if (spawnTimer <= 0)
        {
            if (enemiesToSpawn.Count > 0)
            {
                Vector3 spawnPosition = GetRandomSpawnPosition();

                if (spawnPosition != Vector3.zero)
                {
                    GameObject enemy = Instantiate(enemiesToSpawn[0], spawnPosition, Quaternion.identity);
                    enemiesToSpawn.RemoveAt(0);
                    spawnedEnemies.Add(enemy);
                    spawnTimer = spawnInterval;

                    // Register the enemy with the indicator system
                    WorldSpaceEnemyIndicator indicatorSystem = FindObjectOfType<WorldSpaceEnemyIndicator>();
                    if (indicatorSystem != null)
                    {
                        indicatorSystem.RegisterEnemy(enemy.transform);
                    }
                    else
                    {
                        Debug.LogWarning("WorldSpaceEnemyIndicator not found! Cannot register enemy indicator.");
                    }
                }
            }
            else
            {
                waveTimer = 0;
            }
        }
        else
        {
            spawnTimer -= Time.fixedDeltaTime;
            waveTimer -= Time.fixedDeltaTime;
        }

        if (waveTimer <= 0 && spawnedEnemies.Count <= 0)
        {
            currWave++;
            GenerateWave();
        }
    }

    public void GenerateWave()
    {
        waveValue = currWave * 10;
        GenerateEnemies();

        spawnInterval = waveDuration / enemiesToSpawn.Count;
        waveTimer = waveDuration;
    }

    public void GenerateEnemies()
    {
        List<GameObject> generatedEnemies = new List<GameObject>();
        while (waveValue > 0 || generatedEnemies.Count < 50)
        {
            int randEnemyId = Random.Range(0, enemies.Count);
            int randEnemyCost = enemies[randEnemyId].cost;

            if (waveValue - randEnemyCost >= 0)
            {
                generatedEnemies.Add(enemies[randEnemyId].enemyPrefab);
                waveValue -= randEnemyCost;
            }
            else if (waveValue <= 0)
            {
                break;
            }
        }

        enemiesToSpawn.Clear();
        enemiesToSpawn = generatedEnemies;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomPos = transform.position + new Vector3(
                Random.Range(-spawnRadius, spawnRadius),
                20f,
                Random.Range(-spawnRadius, spawnRadius)
            );

            if (Physics.Raycast(randomPos, Vector3.down, out RaycastHit hit, 50f, groundLayer))
            {
                return hit.point;
            }
        }

        Debug.LogWarning("Failed to find valid spawn position.");
        return Vector3.zero;
    }
}

[System.Serializable]
public class Enemy
{
    public GameObject enemyPrefab;
    public int cost;
}