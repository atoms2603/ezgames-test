using UnityEngine;

public class Retry : BaseButton
{
    protected override void OnClick()
    {
        Debug.Log("Retry game");
        GameManager.Instance.Retry();
    }
}
