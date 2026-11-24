using UnityEngine;

public class CoreHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 200f;

    private float currentHealth;
    private bool isDestroyed;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    
    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isDestroyed)
        {
            return;
        }

        currentHealth = Mathf.Max(currentHealth - damage, 0f);
        Debug.Log($"Core health: {currentHealth}");

        if (currentHealth <= 0f)
        {
            isDestroyed = true;
            Debug.Log("Core destroyed. The run is over.");
        }
    }
}