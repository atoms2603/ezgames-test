using UnityEngine;

public class ModeCampaign : BaseButton
{
    protected override void OnClick()
    {
        Debug.Log("Campaign mode selected");
        GameManager.Instance.StartCampaignMode();
    }
}
