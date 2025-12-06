using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [Header("References")]
    public PlayerInventory playerInventory;

    private PlayerHealth playerHealth;
    private PlayerController playerController;
    private PlayerLevel playerLevel;
    private ConsoleManager consoleManager;

    #region Flat / multiplicative stats (non-weapon)
    [Header("Flat / multiplicative stats (non-weapon)")]
    [Tooltip("Base movement speed multiplier. 1 = default speed.")]
    public float moveSpeed = 1f;

    [Tooltip("Health regeneration per second.")]
    public float healthRegen = 0f;

    [Tooltip("Experience gain multiplier. 1 = normal XP.")]
    public float experienceMod = 1f;

    [Tooltip("Max health multiplier. 1 = base HP.")]
    public float maxHealth = 1f;

    [Tooltip("Damage reduction / armor multiplier.")]
    public float armor = 1f;
    #endregion

    #region BASE weapon bonus percentages (before any upgrades)
    [Header("BASE weapon bonus percentages (before any upgrades)")]
    [Tooltip("Base attack damage bonus in %, before upgrades.")]
    public float baseAttackDamagePercent = 0f;

    [Tooltip("Base projectile speed bonus in %, before upgrades.")]
    public float baseProjectileSpeedPercent = 0f;

    [Tooltip("Base cooldown reduction in %, before upgrades.")]
    public float baseCooldownPercent = 0f;    // e.g. 20 = -20% cooldown

    [Tooltip("Base duration bonus in %, before upgrades.")]
    public float baseDurationPercent = 0f;

    [Tooltip("Base size/area bonus in %, before upgrades.")]
    public float baseSizePercent = 0f;
    #endregion

    #region FINAL weapon bonuses (used by weapons)
    [Header("FINAL weapon bonuses used by weapons (percent)")]
    [Tooltip("Final attack damage bonus in %, e.g. 25 means +25% damage.")]
    public float attackDamage;      // used by TemporaryWeapon

    [Tooltip("Final projectile speed bonus in %, e.g. 10 means +10% speed.")]
    public float projectileSpeed;   // used by GunWeaponController

    [Tooltip("Final cooldown reduction in %, e.g. 20 means -20% cooldown.")]
    public float cooldown;          // used by TemporaryWeapon

    [Tooltip("Final skill duration increase in %, e.g. 15 means +15% duration.")]
    public float duration;          // optional, for future

    [Tooltip("Final area/orbit size increase in %, e.g. 10 means +10% size.")]
    public float size;              // optional, for future
    #endregion

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerController = GetComponent<PlayerController>();
        playerLevel = GetComponent<PlayerLevel>();

        // Apply non-weapon stats on player components (HP, speed, etc.)
        ApplyStatModifiers();

        // Initialize weapon-related percent bonuses from managers
        RecalculateFromUpgrades();

        consoleManager = FindAnyObjectByType<ConsoleManager>();
        if (consoleManager != null)
        {
            RegisterCommands();
        }

        // If you still use ScriptableObject PermanentUpgrade assets
        // this will only affect NON-WEAPON stats (movement, HP, etc.)
        ApplyPermanentUpgrades();

        // If you still use inventory-based temporary upgrades, this
        // will also only affect NON-WEAPON stats.
        ApplyTemporaryUpgrades();
    }

    #region Stat raising / lowering (runtime)
    public void RaiseStat(float amount, StatType statType)
    {
        switch (statType)
        {
            case StatType.AttackDamage:
                // Here we treat 'amount' as percent. Example: amount = 10 => +10% damage.
                baseAttackDamagePercent += amount;
                RecalculateFromUpgrades();
                break;

            case StatType.Armor:
                armor += amount;
                break;

            case StatType.MoveSpeed:
                moveSpeed += amount;
                if (playerController != null)
                {
                    playerController.MoveSpeed *= (1f + amount);
                }
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
    #endregion

    #region Apply stats to components
    private void ApplyStatModifiers()
    {
        if (playerHealth != null)
        {
            playerHealth.maxHealth *= maxHealth;
            // Ensure current HP is adjusted / clamped to new max
            playerHealth.HealDamage(0);
        }

        if (playerController != null)
        {
            playerController.MoveSpeed *= moveSpeed;
        }
    }
    #endregion

    private void Update()
    {
        if (healthRegen > 0f && playerHealth != null)
        {
            playerHealth.HealDamage(healthRegen * Time.deltaTime);
        }

        // Testing hooks were here in original script (commented out)
        // TestUIPart();
    }

    #region New percentage-based upgrade system (weapons)
    /// <summary>
    /// Call this whenever permanent or temporary weapon upgrades change.
    /// This recomputes final weapon bonuses (in percent) from managers.
    /// </summary>
    public void RecalculateFromUpgrades()
    {
        attackDamage = baseAttackDamagePercent + GetTotalPercent("attackDamage");
        projectileSpeed = baseProjectileSpeedPercent + GetTotalPercent("projectileSpd");
        cooldown = baseCooldownPercent + GetTotalPercent("cooldown");
        duration = baseDurationPercent + GetTotalPercent("duration");
        size = baseSizePercent + GetTotalPercent("size");

        Debug.Log($"[PlayerStats] AD:{attackDamage}% ProjSpd:{projectileSpeed}% CD:{cooldown}% Dur:{duration}% Size:{size}%");
    }

    private float GetTotalPercent(string shortKey)
    {
        string permId = "upgrade_" + shortKey;
        string tempId = "temp_" + shortKey;

        float perm = (GameManager.Instance.PermanentUpgradeManager != null)
            ? GameManager.Instance.PermanentUpgradeManager.GetPercent(permId)
            : 0f;

        float temp = (GameManager.Instance.TemporaryUpgradeManager != null)
            ? GameManager.Instance.TemporaryUpgradeManager.GetPercent(tempId)
            : 0f;

        return perm + temp;
    }
    #endregion

    #region Old permanent / temporary upgrade hooks (NON-WEAPON ONLY NOW)
    /// <summary>
    /// Applies permanent upgrades from ScriptableObjects in Resources/PermanentUpgrade.
    /// Now ONLY affects NON-WEAPON stats to avoid double-application.
    /// Weapon stats are handled by PermanentUpgradeManager -> RecalculateFromUpgrades().
    /// </summary>
    public void ApplyPermanentUpgrades()
    {
        var allPermanentUpgrades = Resources.LoadAll<PermanentUpgrade>("PermanentUpgrade");
        if (allPermanentUpgrades == null)
        {
            Debug.LogWarning("No permanent upgrades found in Resources/PermanentUpgrade.");
            return;
        }

        foreach (var upgrade in allPermanentUpgrades)
        {
            int level = upgrade.GetCurrentLevel();
            if (level <= 0) continue;

            float bonus = upgrade.GetBonusAtLevel(level);

            switch (upgrade.displayName)
            {
                // Weapon-related display names intentionally skipped here
                // ("Attack Damage", "Projectile Speed", "Duration", "Cooldown", "Size")
                // because they should be handled by PermanentUpgradeManager.

                case "Movement Speed":
                    moveSpeed += bonus;
                    break;
                case "Health Regen":
                    healthRegen += bonus;
                    break;
                case "XP Amount":
                    experienceMod += bonus;
                    break;
                case "Max Health":
                    maxHealth += bonus;
                    break;
                case "Armour":
                    armor += bonus;
                    break;
            }
        }
    }

    /// <summary>
    /// Applies temporary upgrades from PlayerInventory ScriptableObjects.
    /// Also ONLY affects NON-WEAPON stats now.
    /// Weapon stats come from TemporaryUpgradeManager -> RecalculateFromUpgrades().
    /// </summary>
    public void ApplyTemporaryUpgrades()
    {
        if (playerInventory == null)
            playerInventory = FindAnyObjectByType<PlayerInventory>();
        if (playerInventory == null) return;

        foreach (var item in playerInventory.OwnedItems)
        {
            if (item == null) continue;
            int level = item.CurrentLevel;
            if (level <= 0) continue;

            if (item is TemporaryUpgrade tempUpgrade)
            {
                float bonus = tempUpgrade.GetBonusAtLevel(level);

                switch (tempUpgrade.displayName)
                {
                    // Weapon related here are skipped purposely
                    // ("Attack Damage", "Projectile Speed", "Duration", "Cooldown", "Size")

                    case "Movement Speed":
                        moveSpeed += bonus;
                        break;
                    case "Health Regen":
                        healthRegen += bonus;
                        break;
                    case "XP Amount":
                        experienceMod += bonus;
                        break;
                    case "Max Health":
                        maxHealth += bonus;
                        break;
                    case "Armour":
                        armor += bonus;
                        break;
                }
            }
            else if (item is TemporaryWeapon)
            {
                // Temporary weapons themselves are handled elsewhere
                continue;
            }
        }
    }
    #endregion

    #region Console commands
    private void RegisterCommands()
    {
        consoleManager.RegisterCommand("earnxp", args =>
        {
            if (playerLevel != null)
            {
                if (args.Length > 0 && float.TryParse(args[0], out float xpAmount))
                {
                    playerLevel.GainXP(xpAmount);
                    consoleManager.AppendOutput($"Gained {xpAmount} XP.");
                }
                else if (args.Length == 0)
                {
                    playerLevel.GainXP(100f);
                    consoleManager.AppendOutput("Gained 100 XP.");
                }
                else
                {
                    consoleManager.AppendOutput("Invalid XP amount.");
                }
            }
        }, "<value> - Gain Xp to playerTrans");

        consoleManager.RegisterCommand("enemykill", args =>
        {
            if (args.Length > 0 && args[0] is string enemyType)
            {
                if (playerController != null)
                {
                    playerController.EnemyKilled(enemyType);
                    consoleManager.AppendOutput($"Killed one enemy of type: {enemyType}");
                }
            }
            else
            {
                consoleManager.AppendOutput("Invalid enemy type entered");
            }
        }, "<enemyType> - kill one enemy");

        consoleManager.RegisterCommand("damage", args =>
        {
            if (playerHealth != null)
            {
                if (args.Length > 0 && float.TryParse(args[0], out float damageValue))
                {
                    playerHealth.TakeDamage(damageValue);
                    consoleManager.AppendOutput($"Player takes {damageValue} damage.");
                }
                else if (args.Length == 0)
                {
                    playerHealth.TakeDamage(20);
                    consoleManager.AppendOutput("Player takes 20 damage.");
                }
                else
                {
                    consoleManager.AppendOutput("Invalid damage amount.");
                }
            }
        }, "<value> - damage playerTrans");
    }
    #endregion

    // Testing part from original script was commented out, still available if you need it.
}

/// <summary>
/// High-level stat categories for simple RaiseStat/LowerStat calls.
/// </summary>
public enum StatType
{
    AttackDamage,
    Armor,
    MoveSpeed,
    HealthRegen,
    Experience
}
