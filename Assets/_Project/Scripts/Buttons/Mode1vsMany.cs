using UnityEngine;

public class Mode1vsMany : BaseButton
{
    protected override void OnClick()
    {
        Debug.Log("1 vs many mode selected");
        GameManager.Instance.StartOnevsManyMode();
    }
}
