using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerController controller;

    private void Start()
    {
        controller = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (controller.Movement.IsMoving) return;

        if (other == null) return;

        if (!other.CompareTag("Enemy")) return;

        if (!other.TryGetComponent<NPCController>(out var enemy)) return;

        if (enemy.isKnocked) return;

        if (!enemy.IsEnemy) return;

        controller.Animator.SetTrigger("attack");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(0.7f, 1, 0.7f));
    }
}