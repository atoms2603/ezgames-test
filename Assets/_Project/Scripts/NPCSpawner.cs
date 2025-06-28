using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] Transform enemySpawnArea;
    [SerializeField] Transform teamSpawnArea;
    [SerializeField] List<GameObject> prefabs;

    public List<GameObject> EnemySpawned = new();
    public List<GameObject> TeamSpawned = new();

    private void Start()
    {
        enemySpawnArea = transform.Find("EnemySpawnArea");
        teamSpawnArea = transform.Find("TeamSpawnArea");
        StartCoroutine(SpawnByMode());
    }

    IEnumerator SpawnByMode()
    {
        yield return new WaitUntil(() => GameManager.instance.IsModeSelected);

        switch (GameManager.instance.GameMode)
        {
            case GameMode.OnevsOne:
                SpawnEnemy(1);
                break;
            case GameMode.OnevsMany:
                SpawnEnemy(3);
                break;
            case GameMode.ManyvsMany:
                SpawnEnemy(3);
                SpawnTeam(2);
                break;
        }
    }

    private void SpawnEnemy(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var spawnPosition = GetEnemySpawnPosition();
            var newEnemy = ObjectPoolingManager.Instance.SpawnObject(prefabs[Random.Range(0, prefabs.Count)], spawnPosition, Quaternion.identity);
            newEnemy.GetComponent<NPCController>().Init();
            EnemySpawned.Add(newEnemy);
        }
    }

    private void SpawnTeam(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var spawnPosition = GetTeamSpawnPosition();
            var newEnemy = ObjectPoolingManager.Instance.SpawnObject(prefabs[Random.Range(0, prefabs.Count)], spawnPosition, Quaternion.identity);
            newEnemy.GetComponent<NPCController>().Init(isEnemy: false);
            TeamSpawned.Add(newEnemy);
        }
    }

    private Vector3 GetEnemySpawnPosition()
    {
        MeshRenderer renderer = enemySpawnArea.GetComponent<MeshRenderer>();
        Bounds bounds = renderer.bounds;

        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        float y = bounds.max.y;

        Vector3 spawnPosition = new Vector3(randomX, y, randomZ);
        return spawnPosition;
    }

    private Vector3 GetTeamSpawnPosition()
    {
        MeshRenderer renderer = teamSpawnArea.GetComponent<MeshRenderer>();
        Bounds bounds = renderer.bounds;

        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        float y = bounds.max.y;

        Vector3 spawnPosition = new Vector3(randomX, y, randomZ);
        return spawnPosition;
    }
}
