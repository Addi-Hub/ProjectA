using UnityEngine;
using UnityEngine.InputSystem;

public class TroopSpawner : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GoldManager goldManager;
    [SerializeField] private TroopController guardPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private float guardCost = 10f;

    private InputAction summonGuardAction;

    private void Awake()
    {
        if (playerInput == null)
        {
            Debug.LogError("Assign the Player Input component.", this);
            enabled = false;
            return;
        }

        summonGuardAction =
            playerInput.actions.FindAction("Player/SummonGuard");

        if (summonGuardAction == null)
        {
            Debug.LogError("Could not find Player/SummonGuard.", this);
            enabled = false;
        }
    }

    private void Update()
    {
        if (summonGuardAction.WasPressedThisFrame())
        {
            TrySummonGuard();
        }
    }

    private void TrySummonGuard()
    {
        if (goldManager == null || guardPrefab == null ||
            spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("Assign the gold manager, Guard prefab, and spawn points.", this);
            return;
        }

        if (!goldManager.TrySpendGold(guardCost))
        {
            Debug.Log("Not enough gold to summon a Guard.");
            return;
        }

        if (enemySpawner == null)
        {
            Debug.LogError("Assign the Enemy Spawner.", this);
            return;
        }

        int index = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[index];

        TroopController guard = Instantiate(
            guardPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        guard.Initialize(enemySpawner);
    }
}