using TMPro;
using UnityEngine;

public class CoreHealthDisplay : MonoBehaviour
{
    [SerializeField] private CoreHealth core;
    [SerializeField] private TextMeshProUGUI healthText;

    private int lastDisplayedHealth = -1;

    private void Update()
    {
        if (core == null || healthText == null)
        {
            return;
        }

        int currentHealth = Mathf.CeilToInt(core.CurrentHealth);

        if (currentHealth == lastDisplayedHealth)
        {
            return;
        }

        lastDisplayedHealth = currentHealth;
        int maxHealth = Mathf.CeilToInt(core.MaxHealth);

        healthText.text = $"Core HP: {currentHealth} / {maxHealth}";
    }
}