using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth;
    public float currentHealth;

    [Header("Events")]
    public UnityEvent onTakeDamage;
    public UnityEvent onDie;

    public bool isInvincible = false;

    private void Start()
    {
        maxHealth = 100f * FindAnyObjectByType<PlayerStats>().maxHealth;
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (isInvincible) return;
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        onTakeDamage?.Invoke();

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void HealDamage(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }

    public void SetHealth(float amount)
    {
        if (amount <= 0f)
        {
            currentHealth = 0f;
            Die();
            return;
        }
        if (amount > maxHealth)
        {
            currentHealth = maxHealth;
            return;
        }
        currentHealth = Mathf.Clamp(amount, 0f, maxHealth);
    }

    private void Die()
    {
        // Death logic here
        onDie?.Invoke();
        Debug.Log("Player Died");
    }

    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }
}
