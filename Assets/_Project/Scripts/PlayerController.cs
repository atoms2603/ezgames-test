using UnityEngine;

public class PlayerController : BaseController
{
    public PlayerMovement Movement;
    public PlayerAttack Attack;

    protected override void Start()
    {
        base.Start();
        Movement = GetComponentInChildren<PlayerMovement>();
        Attack = GetComponentInChildren<PlayerAttack>();
        HealthImage.color = Color.green;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (HealthImage != null)
        {
            HealthImage.fillAmount = 1;
            HealthImage.color = Color.green;
        }
    }

    public override void Init(bool isEnemy = true, int level = 1)
    {
        baseHealth = Mathf.RoundToInt(6 * (1f + 0.2f * (level - 1)));
        health = baseHealth;

        speed = Mathf.Clamp(baseSpeed * (1f + 0.03f * (level - 1)), baseSpeed, maxSpeed);
        damage = Mathf.RoundToInt(1.2f * (1f + 0.15f * (level - 1)));
    }
}