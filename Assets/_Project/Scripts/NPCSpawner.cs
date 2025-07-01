using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] Transform enemySpawnArea;
    [SerializeField] Transform teamSpawnArea;
    [SerializeField] Transform playerSpawnPoint;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] List<GameObject> prefabs;
    [SerializeField] int enemyCount;
    [SerializeField] int teamCount;

    public List<GameObject> EnemySpawned = new();
    public List<GameObject> TeamSpawned = new();

    private void Start()
    {
        enemySpawnArea = transform.Find("EnemySpawnArea");
        teamSpawnArea = transform.Find("TeamSpawnArea");
        playerSpawnPoint = transform.Find("PlayerSpawnpoint");
    }

    public void StartSpawn()
    {
        // Clear old enemies
        foreach (var enemy in EnemySpawned)
        {
            enemy.SetActive(false);
        }
        EnemySpawned.Clear();

        // Clear old team
        foreach (var teammate in TeamSpawned)
        {
            teammate.SetActive(false);
        }
        TeamSpawned.Clear();

        enemyCount = 0;
        teamCount = 0;

        ObjectPoolingManager.Instance.ResetPool();
        StartCoroutine(SpawnByMode());
    }

    private IEnumerator SpawnByMode()
    {
        yield return new WaitUntil(() => GameManager.Instance.IsModeSelected);

        switch (GameManager.Instance.GameMode)
        {
            case GameMode.Campaign:
                SpawnPlayer();
                SpawnEnemy(GameManager.Instance.currentLevel);
                break;
            case GameMode.OnevsOne:
                SpawnPlayer();
                SpawnEnemy(1);
                break;
            case GameMode.OnevsMany:
                SpawnPlayer();
                SpawnEnemy(3);
                break;
            case GameMode.ManyvsMany:
                SpawnPlayer();
                SpawnEnemy(3);
                SpawnTeam(2);
                break;
            case GameMode.OneVsAll:
                SpawnPlayer();
                SpawnEnemy(50);
                break;
        }
    }

    private void SpawnEnemy(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var spawnPosition = GetEnemySpawnPosition();
            var newEnemy = ObjectPoolingManager.Instance.SpawnObject(prefabs[Random.Range(0, prefabs.Count)], spawnPosition, Quaternion.identity);
            newEnemy.GetComponent<NPCController>().Init(isEnemy: true);
            newEnemy.GetComponent<NPCController>().OnDeath += OnEnemyDeathHandler;
            EnemySpawned.Add(newEnemy);
            enemyCount++;
        }
    }

    private void SpawnTeam(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var spawnPosition = GetTeamSpawnPosition();
            var newEnemy = ObjectPoolingManager.Instance.SpawnObject(prefabs[Random.Range(0, prefabs.Count)], spawnPosition, Quaternion.identity);
            newEnemy.GetComponent<NPCController>().Init(isEnemy: false);
            newEnemy.GetComponent<NPCController>().OnDeath += OnTeamDeathHandler;
            TeamSpawned.Add(newEnemy);
            teamCount++;
        }
    }

    private void SpawnPlayer()
    {
        var player = ObjectPoolingManager.Instance.SpawnObject(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
        player.GetComponent<PlayerController>().Init();
        player.GetComponent<PlayerController>().OnDeath += OnTeamDeathHandler;
        TeamSpawned.Add(player);
        teamCount++;
    }

    private void OnEnemyDeathHandler()
    {
        enemyCount--;

        switch (GameManager.Instance.GameMode)
        {
            case GameMode.Campaign:
                if (enemyCount <= 0)
                {
                    if (GameManager.Instance.currentLevel >= GameManager.Instance.maxLevel)
                    {
                        ResetGame();
                    }
                    else
                    {
                        GameManager.Instance.currentLevel++;
                        StartCoroutine(GameManager.Instance.StartNewLevel());
                    }
                }
                break;
            default:
                if (enemyCount <= 0)
                {
                    ResetGame();
                }
                break;
        }
    }

    private void OnTeamDeathHandler()
    {
        teamCount--;
        if (teamCount <= 0)
        {
            ResetGame();
        }
    }

    private void ResetGame()
    {
        GameManager.Instance.currentLevel = 1;
        UIManager.Instance.ShowEndGamePanel(true);
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
