using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BaseController : MonoBehaviour
{
    [Header("References")]
    public Animator Animator;
    public Collider Collider;
    public Rigidbody Rigidbody;
    public Image HealthImage;

    [Header("Stats")]
    public int baseHealth = 5;
    public int health;
    public int damage = 1;
    public int baseDamage = 1;
    public bool isKnocked = false;
    public float speed = 1f;
    public float baseSpeed = 2f;
    public float maxSpeed = 5f;
    public int rotateSpeed = 10;
    public float attackRange = 0.5f;
    public bool isMoving = false;

    public event Action OnDeath;

    protected virtual void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Collider = GetComponent<Collider>();
        Animator = GetComponentInChildren<Animator>();
        HealthImage = transform.Find("HealthCanvas").GetComponentInChildren<Image>();
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

    public virtual void Init(bool isEnemy = true, int level = 1)
    {

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

        HealthImage.fillAmount = Mathf.Clamp01((float)health / baseHealth);
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