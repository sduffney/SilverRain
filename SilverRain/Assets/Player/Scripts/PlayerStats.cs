using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float attackDamage = 10f;
    public float projectileSpeed = 20f;
    public float duration = 5f;
    public float cooldown = 0.5f;
    public float size = 10f;
    public float moveSpeed = 5f;
    public float healthRegen = 0f;
    public float experienceMod = 1f;
    public float maxHealth = 100f;
    public float armor = 0f;

    private PlayerHealth playerHealth;
    private PlayerController playerController;
    private PlayerLevel playerLevel;

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerController = GetComponent<PlayerController>();
        playerLevel = GetComponent<PlayerLevel>();

        ApplyStatModifiers();
    }

    public void RaiseStat(float amount, StatType statType)
    {
        switch (statType)
        {
            case StatType.AttackDamage:
                attackDamage += amount;
                break;
            case StatType.Armor:
                armor += amount;
                break;
            case StatType.MoveSpeed:
                moveSpeed += amount;
                playerController.moveSpeed *= (1f + amount);
                break;
            case StatType.HealthRegen:
                healthRegen += amount;
                break;
            case StatType.Experience:
                experienceMod += amount;
                break;
        }
    }

    public void LowerStat(float amount, StatType statType)
    {
        RaiseStat(-amount, statType);
    }

    private void ApplyStatModifiers()
    {
        if (playerHealth != null)
        {
            playerHealth.maxHealth *= maxHealth;
            playerHealth.HealDamage(0);
        }

        if (playerController != null)
        {
            playerController.moveSpeed *= moveSpeed;
        }
    }

    private void Update()
    {
        if (healthRegen > 0 && playerHealth != null)
        {
            playerHealth.HealDamage(healthRegen * Time.deltaTime);
        }
    }
}

public enum StatType
{
    AttackDamage,
    Armor,
    MoveSpeed,
    HealthRegen,
    Experience
}