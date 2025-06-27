using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator Animator;
    public Rigidbody Rigidbody;

    public PlayerMovement Movement;
    public PlayerAttack Attack;

    [Header("Stats")]
    [SerializeField] private int baseHealth = 5;
    [SerializeField] private int health;
    [SerializeField] private int damage = 1;
    [SerializeField] private bool isKnocked = false;

    public int Damage => damage;
    public bool IsKnocked => isKnocked;

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();
        Movement = GetComponentInChildren<PlayerMovement>();
        Attack = GetComponentInChildren<PlayerAttack>();
        health = baseHealth;
    }

    public void ApplyDamage(int damage)
    {
        Animator.SetTrigger("hit");
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            isKnocked = true;
            Animator.SetTrigger("knocked");
            Animator.SetBool("isKnocked", isKnocked);
        }
    }
}