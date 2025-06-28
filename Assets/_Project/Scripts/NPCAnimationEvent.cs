using UnityEngine;

public class NPCAnimationEvent : MonoBehaviour
{
    public void HitEnemy()
    {
        var halfExtents = new Vector3(0.7f, 1f, 0.7f);
        var enemies = Physics.OverlapBox(transform.position, halfExtents);

        var selfController = transform.GetComponentInParent<BaseController>();
        if (selfController == null) return;

        bool isEnemy = selfController is NPCController npcSelf && npcSelf.IsEnemy;

        foreach (Collider enemy in enemies)
        {
            if (!enemy.TryGetComponent<BaseController>(out var target)) continue;

            if (target.IsKnocked || target == selfController) continue;

            if (target is PlayerController player)
            {
                player.ApplyDamage(selfController.Damage);
                continue;
            }

            // Check if hitting another NPC with opposite faction
            if (target is NPCController npcTarget)
            {
                if (npcTarget.IsEnemy != isEnemy)
                {
                    npcTarget.ApplyDamage(selfController.Damage);
                }
            }
        }
    }
}
