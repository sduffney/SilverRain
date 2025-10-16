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

        // For testing purposes only
        TestUIPart();
    }

    // Testing purposes only
    public int score = 0;
    private void TestUIPart()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerHealth.TakeDamage(20f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerHealth.HealDamage(20f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            playerLevel.GainXP(50);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            playerController.AddBuff("buff");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            playerController.EnemyKilled("Type1");
            score += 100;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            playerController.EnemyKilled("Type2");
            score += 150;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            playerController.EnemyKilled("Type3");
            score += 250;
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