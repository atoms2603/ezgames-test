using System;
using System.Collections;
using UnityEngine;

public enum GameMode
{
    Campaign,
    OnevsOne,
    OnevsMany,
    ManyvsMany,
    OneVsAll
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameMode GameMode;

    public int currentLevel = 1;
    public int maxLevel = 10;

    public bool IsModeSelected = false;
    public bool IsGameStarted = false;

    public Action OnGameStart;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void StartCampaignMode()
    {
        GameMode = GameMode.Campaign;
        StartCoroutine(StartNewLevel());
    }

    public void StartOnevsOneMode()
    {
        GameMode = GameMode.OnevsOne;
        StartCoroutine(StartNewLevel());
    }

    public void StartOnevsManyMode()
    {
        GameMode = GameMode.OnevsMany;
        StartCoroutine(StartNewLevel());
    }

    public void StartManyvsManyMode()
    {
        GameMode = GameMode.ManyvsMany;
        StartCoroutine(StartNewLevel());
    }

    public void StartOneVsAllMode()
    {
        GameMode = GameMode.OneVsAll;
        StartCoroutine(StartNewLevel());
    }

    public void Back()
    {
        UIManager.Instance.ShowEndGamePanel(false);
        UIManager.Instance.ShowSelectionMode(true);
    }

    public void Retry()
    {
        StartCoroutine(StartNewLevel());
        UIManager.Instance.ShowEndGamePanel(false);
    }

    public IEnumerator StartNewLevel()
    {
        IsGameStarted = false;
        UIManager.Instance.ShowSelectionMode(false);
        IsModeSelected = true;

        UIManager.Instance.ShowCountDownText(true, GetTitle());
        NPCSpawner spawner = FindAnyObjectByType<NPCSpawner>();
        if (spawner != null)
        {
            spawner.StartSpawn();
        }

        yield return new WaitForSeconds(1);

        int countDown = 3;
        while (countDown > 0)
        {
            UIManager.Instance.ShowCountDownText(true, countDown.ToString());
            yield return new WaitForSeconds(1);
            countDown--;
        }

        IsGameStarted = true;
        OnGameStart?.Invoke();
    }

    private string GetTitle() => GameMode switch
    {
        GameMode.Campaign => $"Level {currentLevel}",
        GameMode.OnevsOne => "1 vs 1",
        GameMode.OnevsMany => "1 vs Many",
        GameMode.ManyvsMany => "Many vs Many",
        GameMode.OneVsAll => "1 vs all (50)",
        _ => ""
    };
}
