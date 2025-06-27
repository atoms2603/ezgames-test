using UnityEngine;

public class EnemyAnimationEvent : MonoBehaviour
{
    public void HitEnemy()
    {
        Collider[] enemies = Physics.OverlapBox(transform.position, new Vector3(0.7f, 1, 0.7f));
        foreach (Collider enemy in enemies)
        {
            if (enemy.CompareTag("Player") || enemy.CompareTag("PlayerTeam"))
            {
                Debug.Log("Hit: " + enemy.transform.name);
                if(enemy.TryGetComponent<PlayerController>(out var controller))
                {
                    if (controller.IsKnocked) continue;
                    var damage = transform.GetComponentInParent<EnemyController>();
                    controller.ApplyDamage(damage.Damage);
                }
            }
        }
    }
}
