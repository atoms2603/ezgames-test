using System;
using System.Collections;
using UnityEngine;

public enum GameMode
{
    OnevsOne,
    OnevsMany,
    ManyvsMany,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameMode GameMode;

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

    public void Start1vs1Mode()
    {
        GameMode = GameMode.OnevsOne;
        StartCoroutine(StartCountDown());
    }

    public void Start1vsManyMode()
    {
        GameMode = GameMode.OnevsMany;
        StartCoroutine(StartCountDown());
    }

    public void StartManyvsManyMode()
    {
        GameMode = GameMode.ManyvsMany;
        StartCoroutine(StartCountDown());
    }

    public void Back()
    {
        UIManager.Instance.ShowEndGamePanel(false);
        UIManager.Instance.ShowSelectionMode(true);
    }

    public void Retry()
    {
        StartCoroutine(StartCountDown());
        UIManager.Instance.ShowEndGamePanel(false);
    }

    IEnumerator StartCountDown()
    {
        IsGameStarted = false;
        UIManager.Instance.ShowSelectionMode(false);
        IsModeSelected = true;

        NPCSpawner spawner = FindAnyObjectByType<NPCSpawner>();
        if (spawner != null)
        {
            spawner.StartSpawn();
        }

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
}
