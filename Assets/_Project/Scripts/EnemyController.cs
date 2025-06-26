using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void ApplyDamage()
    {
        animator.SetTrigger("hit");
    }
}
