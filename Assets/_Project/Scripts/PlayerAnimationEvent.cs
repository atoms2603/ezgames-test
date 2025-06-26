using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    public void HitEnemy()
    {
        Collider[] enemies = Physics.OverlapBox(transform.position, new Vector3(0.7f, 1, 0.7f));
        foreach (Collider enemy in enemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                Debug.Log("Enemy hit: " + enemy.transform.name);
                if(enemy.TryGetComponent<EnemyController>(out var controller))
                {
                    controller.ApplyDamage();
                }
            }
        }
    }
}
