using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCController : BaseController
{
    [Header("Target")]
    [SerializeField] private Transform target;

    public bool IsEnemy;

    public void Init(bool isEnemy = true, int level = 1)
    {
        IsEnemy = isEnemy;
        health = level == 1 ? baseHealth : Mathf.RoundToInt(baseHealth * level * 0.5f);
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

        if (isKnocked || target.GetComponent<BaseController>().IsKnocked)
        {
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

        isMoving = false;
        Animator.SetBool("isWalking", isMoving);
        if (playerController.IsKnocked) return;

        Animator.SetTrigger("attack");
    }

    private void FindTarget()
    {
        var npcs = new List<NPCController>(FindObjectsByType<NPCController>(sortMode: FindObjectsSortMode.None)).Where(x => !x.IsKnocked);

        List<NPCController> opponents = new();
        foreach (var npc in npcs)
        {
            if (npc == this || npc.IsKnocked) continue;

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