using UnityEngine;

public class GoldManager : MonoBehaviour
{
    [SerializeField] private float startingGold = 0f;
    [SerializeField] private float goldPerSecond = 1f;
    [SerializeField] private float currentGold;

    public float CurrentGold => currentGold;

    private void Awake()
    {
        currentGold = startingGold;
    }

    private void Update()
    {
        currentGold += goldPerSecond * Time.deltaTime;
    }

    public bool TrySpendGold(float amount)
    {
        if (amount <= 0f || currentGold < amount)
        {
            return false;
        }

        currentGold -= amount;
        return true;
    }
}