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
    }
}