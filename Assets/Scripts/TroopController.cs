using UnityEngine;

public class TroopController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackDamage = 5f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float targetRefreshInterval = 0.5f;

    private EnemySpawner enemySpawner;
    private EnemyController currentTarget;
    private CharacterController characterController;
    private float nextTargetSearchTime;
    private float nextAttackTime;

    public void Initialize(EnemySpawner spawner)
    {
        enemySpawner = spawner;
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (enemySpawner == null)
        {
            return;
        }

        if (Time.time >= nextTargetSearchTime)
        {
            currentTarget = enemySpawner.GetNearestEnemy(transform.position);
            nextTargetSearchTime = Time.time + targetRefreshInterval;
        }

        if (currentTarget == null)
        {
            return;
        }

        Vector3 targetPosition = currentTarget.transform.position;
        targetPosition.y = transform.position.y;

        Vector3 direction = targetPosition - transform.position;

        if (direction.sqrMagnitude > attackRange * attackRange)
        {
            MoveTowards(targetPosition, direction);
        }
        else
        {
            TryAttack();
        }
    }

    private void MoveTowards(Vector3 targetPosition, Vector3 direction)
    {
        float distance = direction.magnitude;
        float distanceToMove = Mathf.Min(
            moveSpeed * Time.deltaTime,
            distance - attackRange
        );

        characterController.Move(direction.normalized * distanceToMove);

        if (direction.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private void TryAttack()
    {
        if (Time.time < nextAttackTime)
        {
            return;
        }

        EnemyHealth enemyHealth =
            currentTarget.GetComponent<EnemyHealth>();

        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(attackDamage);
        }

        nextAttackTime = Time.time + attackCooldown;
    }
}