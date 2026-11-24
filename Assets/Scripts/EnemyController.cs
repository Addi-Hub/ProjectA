using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private CoreHealth core;
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackDamage = 5f;
    [SerializeField] private float attackCooldown = 1.5f;
    private Collider coreCollider;

    private CharacterController characterController;
    private float nextAttackTime;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        if (core != null)
        {
            coreCollider = core.GetComponent<Collider>();
        }
    }

    public void SetCoreTarget(CoreHealth target)
    {
        core = target;
        coreCollider = core != null ? core.GetComponent<Collider>() : null;
    }

    private void Update()
    {
        if (core == null || coreCollider == null)
        {
            return;
        }

        Vector3 targetPoint = coreCollider.ClosestPoint(transform.position);
        targetPoint.y = transform.position.y;

        Vector3 direction = targetPoint - transform.position;
        float distance = direction.magnitude;

        if (distance > attackRange)
        {
            MoveToCore(direction, distance);
        }
        else
        {
            TryAttackCore();
        }
    }

    private void MoveToCore(Vector3 direction, float distance)
    {
        float distanceToMove = Mathf.Min(
            moveSpeed * Time.deltaTime,
            distance - attackRange
        );

        if (distanceToMove <= 0f)
        {
            return;
        }

        characterController.Move(direction.normalized * distanceToMove);

        if (direction.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private void TryAttackCore()
    {
        if (Time.time < nextAttackTime)
        {
            return;
        }

        core.TakeDamage(attackDamage);
        nextAttackTime = Time.time + attackCooldown;
    }
}