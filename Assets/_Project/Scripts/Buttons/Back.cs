using UnityEngine;

public class Back : BaseButton
{
    protected override void OnClick()
    {
        Debug.Log("Back to mode selection");
        GameManager.Instance.Back();
    }
}
