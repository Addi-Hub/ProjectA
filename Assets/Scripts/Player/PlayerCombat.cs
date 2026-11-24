using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 0.5f;

    private PlayerInput playerInput;
    private InputAction attackAction;
    private float nextAttackTime;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        attackAction = playerInput.actions.FindAction("Player/Attack");

        if (attackAction == null)
        {
            Debug.LogError("Could not find the Player/Attack action.", this);
        }
    }

    private void Update()
    {
        if (attackAction != null && attackAction.WasPressedThisFrame())
        {
            TryAttack();
        }
    }

    private void TryAttack()
    {
        if (attackPoint == null || Time.time < nextAttackTime)
        {
            return;
        }

        nextAttackTime = Time.time + attackCooldown;

        Collider[] hitColliders =
            Physics.OverlapSphere(attackPoint.position, attackRange);

        foreach (Collider hitCollider in hitColliders)
        {
            EnemyHealth enemy =
                hitCollider.GetComponentInParent<EnemyHealth>();

            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}