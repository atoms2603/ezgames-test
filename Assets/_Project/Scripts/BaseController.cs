using System;
using System.Collections;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    public Animator Animator;
    public Collider Collider;
    public Rigidbody Rigidbody;

    [Header("Stats")]
    public int baseHealth = 5;
    public int health;
    public int damage = 1;
    public bool isKnocked = false;
    public int speed = 1;
    public int rotateSpeed = 10;
    public float attackRange = 1f;
    public bool isMoving = false;

    public int Damage => damage;
    public bool IsKnocked => isKnocked;

    public event Action OnDeath;

    protected virtual void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Collider = GetComponent<Collider>();
        Animator = GetComponentInChildren<Animator>();
    }

    protected virtual void OnEnable()
    {
        if (Rigidbody != null)
        {
            Rigidbody.isKinematic = false;
        }

        if (Collider != null)
        {
            Collider.enabled = true;
        }
    }

    public virtual void ApplyDamage(int damage)
    {
        Animator.SetTrigger("hit");
        health -= damage;
        if (health <= 0)
        {
            health = 0;

            StartCoroutine(KnockedOut());
        }
    }

    private void TriggerOnDeathEvent()
    {
        OnDeath?.Invoke();
        OnDeath = null;
    }

    protected virtual IEnumerator KnockedOut()
    {
        isKnocked = true;
        Animator.SetTrigger("knocked");
        Animator.SetBool("isKnocked", isKnocked);
        TriggerOnDeathEvent();
        if (Rigidbody != null)
        {
            Rigidbody.isKinematic = true;
        }

        if (Collider != null)
        {
            Collider.enabled = false;
        }

        yield return new WaitForSeconds(2);

        isKnocked = false;
        Animator.SetBool("isKnocked", isKnocked);
        ObjectPoolingManager.Instance.ReturnObjectToPool(gameObject);
    }
}