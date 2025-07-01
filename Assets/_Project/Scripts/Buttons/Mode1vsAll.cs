using UnityEngine;

public class Mode1vsAll : BaseButton
{
    protected override void OnClick()
    {
        Debug.Log("1 vs all mode selected");
        GameManager.Instance.StartOneVsAllMode();
    }
}
