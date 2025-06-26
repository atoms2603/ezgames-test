using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator Animator;
    public Rigidbody Rigidbody;

    public PlayerMovement Movement;
    public PlayerAttack Attack;

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();
        Movement = GetComponentInChildren<PlayerMovement>();
        Attack = GetComponentInChildren<PlayerAttack>();
    }
}