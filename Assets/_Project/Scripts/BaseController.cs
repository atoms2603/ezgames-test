using UnityEngine;

public class BaseController : MonoBehaviour
{
    public Animator Animator;
    public Rigidbody Rigidbody;

    [Header("Stats")]
    public int baseHealth = 5;
    public int health;
    public int damage = 1;
    public bool isKnocked = false;
    public int speed = 1;
    public int rotateSpeed = 10;
    public float attackRange = 0.5f;
    public bool isMoving = false;

    public int Damage => damage;
    public bool IsKnocked => isKnocked;

    protected virtual void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();
        health = baseHealth;
    }

    public virtual void ApplyDamage(int damage)
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