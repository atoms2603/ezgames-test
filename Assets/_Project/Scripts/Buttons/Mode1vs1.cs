using UnityEngine;

public class Mode1vs1 : BaseButton
{
    protected override void OnClick()
    {
        Debug.Log("1 vs 1 mode selected");
        GameManager.instance.Start1vs1Mode();
    }
}
