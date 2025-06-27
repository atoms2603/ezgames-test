using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class EnemyController : MonoBehaviour
{
    public Animator Animator;
    public Rigidbody Rigidbody;

    [Header("Stats")]
    [SerializeField] private int baseHealth = 5;
    [SerializeField] private int health;
    [SerializeField] private int damage = 1;
    [SerializeField] private int speed = 1;
    [SerializeField] private int rotateSpeed = 10;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private bool isKnocked = false;
    [SerializeField] private bool isMoving = false;

    [Header("Target")]
    [SerializeField] private Transform player;

    public int Damage => damage;
    public bool IsKnocked => isKnocked;

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();

        player = GameObject.FindWithTag("Player").transform;

        health = baseHealth;
    }

    private void FixedUpdate()
    {
        if (isKnocked)
        {
            isMoving = false;
            Animator.SetBool("isWalking", isMoving);
            return;
        }

        Animator.SetTrigger("attack");
        //Movement();
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

    private void Movement()
    {
        var distance = Vector3.Distance(player.position, transform.position);
        if (distance <= attackRange)
        {
            if (!player.TryGetComponent<PlayerController>(out var playerController)) return;

            if (playerController.IsKnocked) return;

            isMoving = false;
            Animator.SetBool("isWalking", isMoving);
            Animator.SetTrigger("attack");
            return;
        }

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;
        direction.Normalize();

        Rigidbody.linearVelocity = direction * speed;

        if (!isKnocked && direction.sqrMagnitude > 0.01f)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotateSpeed * Time.fixedDeltaTime);
        }

        isMoving = true;
        Animator.SetBool("isWalking", isMoving);
    }
}
