using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCController : BaseController
{
    [Header("Target")]
    [SerializeField] private Transform target;

    public bool IsEnemy;

    protected override void Start()
    {
        base.Start();
        SetHealth();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SetHealth();
    }

    private void SetHealth()
    {
        if (HealthImage != null)
        {
            HealthImage.fillAmount = 1;
            if (IsEnemy)
            {
                HealthImage.color = Color.red;
            }
            else
            {
                HealthImage.color = Color.blue;
            }
        }
    }

    public override void Init(bool isEnemy = true, int level = 1)
    {
        IsEnemy = isEnemy;

        baseHealth = Mathf.RoundToInt(5 * (1f + 0.3f * (level - 1)));
        health = baseHealth;

        speed = Mathf.Clamp(baseSpeed * (1f + 0.05f * (level - 1)), baseSpeed, 6f);
        damage = Mathf.RoundToInt(baseDamage * (1f + 0.2f * (level - 1)));

        SetHealth();
        target = null;
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsGameStarted) return;

        if (target == null || target.GetComponent<BaseController>().isKnocked || target.gameObject.activeSelf == false)
        {
            FindTarget();
        }

        if (target == null)
        {
            return;
        }

        if (isKnocked || target.GetComponent<BaseController>().isKnocked)
        {
            Rigidbody.linearVelocity = Vector3.zero;
            isMoving = false;
            Animator.SetBool("isWalking", isMoving);
            return;
        }

        Movement();
    }

    private void Movement()
    {
        var distance = Vector3.Distance(target.position, transform.position);
        if (distance <= attackRange)
        {
            TryAttack();
            return;
        }

        Vector3 direction = target.position - transform.position;
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

    private void TryAttack()
    {
        if (!target.TryGetComponent<BaseController>(out var playerController)) return;

        Rigidbody.linearVelocity = Vector3.zero;
        isMoving = false;
        Animator.SetBool("isWalking", isMoving);
        if (playerController.isKnocked) return;

        Animator.SetTrigger("attack");
    }

    private void FindTarget()
    {
        var npcs = new List<NPCController>(FindObjectsByType<NPCController>(sortMode: FindObjectsSortMode.None)).Where(x => !x.isKnocked);

        List<NPCController> opponents = new();
        foreach (var npc in npcs)
        {
            if (npc == this || npc.isKnocked) continue;

            bool isOpponent = IsEnemy != npc.IsEnemy;
            if (isOpponent)
            {
                opponents.Add(npc);
            }
        }

        if (opponents.Count > 0)
        {
            target = opponents[UnityEngine.Random.Range(0, opponents.Count)].transform;
        }
        else if (IsEnemy)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                target = playerObj.transform;
            }
        }
    }
}