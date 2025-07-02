using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private Canvas canvas;
    [SerializeField] private DynamicJoystick joystick;
    [SerializeField] private GameObject modeSelectPanel;
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private GameObject howToPlay;
    [SerializeField] private TextMeshProUGUI CountDownText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        joystick = FindAnyObjectByType<DynamicJoystick>(FindObjectsInactive.Include);
        joystick.gameObject.SetActive(false);

        howToPlay = canvas.transform.Find("HowToPlay").gameObject;
        howToPlay.SetActive(false);

        CountDownText = canvas.transform.Find("CountDownText").GetComponent<TextMeshProUGUI>();
        CountDownText.gameObject.SetActive(false);

        endGamePanel = canvas.transform.Find("EndGamePanel").gameObject;
        endGamePanel.SetActive(false);

        modeSelectPanel = canvas.transform.Find("ModeSelectPanel").gameObject;
        modeSelectPanel.SetActive(true);

        GameManager.Instance.OnGameStart += ShowGameplayUI;
    }

    public void ShowSelectionMode(bool isShow)
    {
        if (modeSelectPanel == null) return;
        modeSelectPanel.SetActive(isShow);
    }

    public void ShowCountDownText(bool isShow, string text)
    {
        if (CountDownText == null) return;
        CountDownText.gameObject.SetActive(isShow);
        CountDownText.SetText(text);
    }

    public void ShowEndGamePanel(bool isShow)
    {
        if (endGamePanel == null) return;
        endGamePanel.gameObject.SetActive(isShow);
        DisableGameplayUI();
    }

    private void ShowGameplayUI()
    {
        if (joystick != null)
        {
            joystick.gameObject.SetActive(true);
        }

        if (howToPlay != null)
        {
            howToPlay.SetActive(true);
        }

        if (CountDownText != null && CountDownText.gameObject.activeSelf)
        {
            CountDownText.gameObject.SetActive(false);
        }
    }

    private void DisableGameplayUI()
    {
        if (joystick != null)
        {
            joystick.gameObject.SetActive(false);
        }

        if (howToPlay != null)
        {
            howToPlay.SetActive(false);
        }

        if (CountDownText != null && CountDownText.gameObject.activeSelf)
        {
            CountDownText.gameObject.SetActive(false);
        }
    }
}
