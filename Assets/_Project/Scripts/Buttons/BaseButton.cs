using UnityEngine;
using UnityEngine.UI;

public abstract class BaseButton : MonoBehaviour
{
    [SerializeField] protected Button button;

    protected virtual void Start()
    {
        button = GetComponent<Button>();
        AddOnClickEvent();
    }

    protected abstract void OnClick();

    protected void AddOnClickEvent()
    {
        button.onClick.AddListener(OnClick);
    }
}
