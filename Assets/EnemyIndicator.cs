using UnityEngine;
using System.Collections.Generic;

public class WorldSpaceEnemyIndicator : MonoBehaviour
{
    public Transform player;
    public GameObject indicatorPrefab;
    public float indicatorDistanceFromPlayer = 3f;
    public float enemyVisibleRange = 10f;
    public float yOffset = 2f;

    private Dictionary<Transform, GameObject> enemyIndicators = new Dictionary<Transform, GameObject>();
    private List<Transform> enemies = new List<Transform>();

    public void RegisterEnemy(Transform enemy)
    {
        if (enemyIndicators.ContainsKey(enemy)) return;

        enemies.Add(enemy);

        GameObject indicator = Instantiate(indicatorPrefab);
        indicator.name = $"Indicator_for_{enemy.name}";
        indicator.transform.position = player.position + Vector3.up * yOffset;
        indicator.SetActive(false);

        enemyIndicators.Add(enemy, indicator);

        Debug.Log($"[Indicator Registered] for {enemy.name}");
    }

 void Update()
{
    // Cleanup any destroyed enemies
    for (int i = enemies.Count - 1; i >= 0; i--)
    {
        if (enemies[i] == null)
        {
            Transform removed = enemies[i];
            enemies.RemoveAt(i);

            if (enemyIndicators.TryGetValue(removed, out GameObject indicatorToRemove))
            {
                Destroy(indicatorToRemove);
                enemyIndicators.Remove(removed);
            }

            continue;
        }

        Transform enemy = enemies[i];
        float distance = Vector3.Distance(player.position, enemy.position);
        GameObject indicator = enemyIndicators[enemy];

        if (distance < enemyVisibleRange)
        {
            indicator.SetActive(false);
            continue;
        }

        indicator.SetActive(true);

        Vector3 direction = (enemy.position - player.position);
        direction.y = 0;
        direction.Normalize();

        Vector3 indicatorPosition = player.position + direction * indicatorDistanceFromPlayer;
        indicatorPosition.y += yOffset;

        indicator.transform.position = indicatorPosition;
        indicator.transform.LookAt(enemy.position);
    }
}
}