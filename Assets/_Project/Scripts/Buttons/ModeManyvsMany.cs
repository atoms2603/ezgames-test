using UnityEngine;

public class ModeManyvsMany : BaseButton
{
    protected override void OnClick()
    {
        Debug.Log("Many vs many mode selected");
        GameManager.Instance.StartManyvsManyMode();
    }
}
