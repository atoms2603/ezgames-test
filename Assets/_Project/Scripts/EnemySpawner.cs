using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform spawnArea;
    [SerializeField] List<GameObject> prefabs;

    public List<GameObject> Spawned = new();

    private void Start()
    {
        spawnArea = transform.Find("SpawnArea");
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
                SpawnEnemy(5);
                break;
        }
    }

    private void SpawnEnemy(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var spawnPosition = GetSpawnPosition();
            var newEnemy = ObjectPoolingManager.Instance.SpawnObject(prefabs[Random.Range(0, prefabs.Count)], spawnPosition, Quaternion.identity);
            Spawned.Add(newEnemy);
        }
    }

    private Vector3 GetSpawnPosition()
    {
        MeshRenderer renderer = spawnArea.GetComponent<MeshRenderer>();
        Bounds bounds = renderer.bounds;

        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        float y = bounds.max.y;

        Vector3 spawnPosition = new Vector3(randomX, y, randomZ);
        return spawnPosition;
    }
}
