using UnityEngine;

public class CoreTurret : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private float attackRange = 8f;
    [SerializeField] private float attackDamage = 4f;
    [SerializeField] private float attackCooldown = 1.5f;

    private float nextAttackTime;

    private void Update()
    {
        if (enemySpawner == null || Time.time < nextAttackTime)
        {
            return;
        }

        EnemyController target =
            enemySpawner.GetNearestEnemy(transform.position);

        if (target == null)
        {
            return;
        }

        float distanceSquared =
            (target.transform.position - transform.position).sqrMagnitude;

        if (distanceSquared > attackRange * attackRange)
        {
            return;
        }

        EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();

        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(attackDamage);
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}