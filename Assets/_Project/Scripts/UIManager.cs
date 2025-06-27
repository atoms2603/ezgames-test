using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private Canvas canvas;
    [SerializeField] private DynamicJoystick joystick;
    [SerializeField] private GameObject modeSelectPanel;
    [SerializeField] private TextMeshProUGUI CountDownText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        joystick = FindAnyObjectByType<DynamicJoystick>(FindObjectsInactive.Include);
        joystick.gameObject.SetActive(false);

        CountDownText = canvas.transform.Find("CountDownText").GetComponent<TextMeshProUGUI>();
        CountDownText.gameObject.SetActive(false);

        GameManager.instance.OnGameStart += ShowGameplayUI;
    }

    public void ShowSelectionMode(bool isShow)
    {
        canvas.transform.Find("ModeSelectPanel").gameObject.SetActive(isShow);
    }

    public void ShowCountDownText(bool isShow, string text)
    {
        CountDownText.gameObject.SetActive(isShow);
        CountDownText.SetText(text);
    }

    private void ShowGameplayUI()
    {
        joystick.gameObject.SetActive(true);
        if(CountDownText != null && CountDownText.gameObject.activeSelf)
        {
            CountDownText.gameObject.SetActive(false);
        }
    }
}
