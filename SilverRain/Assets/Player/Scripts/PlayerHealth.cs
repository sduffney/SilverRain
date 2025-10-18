using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth;
    public float currentHealth;

    [Header("Events")]
    public UnityEvent onDie;

    private void Start()
    {
        maxHealth = FindAnyObjectByType<PlayerStats>().maxHealth;
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

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
