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
    public static GameManager instance;

    public GameMode GameMode;

    public bool IsModeSelected = false;
    public bool IsGameStarted = false;

    public Action OnGameStart;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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

    IEnumerator StartCountDown()
    {
        UIManager.instance.ShowSelectionMode(false);
        IsModeSelected = true;

        int countDown = 3;
        while (countDown > 0)
        {
            UIManager.instance.ShowCountDownText(true, countDown.ToString());
            yield return new WaitForSeconds(1);
            countDown--;
        }

        IsGameStarted = true;
        OnGameStart?.Invoke();
    }
}
