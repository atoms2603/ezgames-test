using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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
        StartCoroutine(StartCountDown());
    }

    public void Start1vsManyMode()
    {
        StartCoroutine(StartCountDown());
    }

    public void StartManyvsManyMode()
    {
        StartCoroutine(StartCountDown());
    }

    IEnumerator StartCountDown()
    {
        UIManager.instance.ShowSelectionMode(false);

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
