using UnityEngine;
using UnityEngine.Playables;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }
    public PlayerInventory playerInventory;

    public float attackDamage = 1f;     // Base attack damage multiplier
    public float projectileSpeed = 1f;  // Base projectile speed multiplier
    public float duration = 1f;         // Base duration multiplier <---------------- ?
    public float cooldown = 1f;         // Base weapon cooldown multiplier
    public float size = 1f;             // Base attack size multiplier
    public float moveSpeed = 1f;        // Base movement speed multiplier
    public float healthRegen = 0f;      // Health regeneration per second
    public float experienceMod = 1f;    // Experience gain multiplier
    public float maxHealth = 1f;        // Max health multiplier
    public float armor = 1f;            // Damage reduction multiplier

    //public int score = 0;

    private PlayerHealth playerHealth;
    private PlayerController playerController;
    private PlayerLevel playerLevel;

    private ConsoleManager consoleManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerController = GetComponent<PlayerController>();
        playerLevel = GetComponent<PlayerLevel>();

        ApplyStatModifiers();
        
        consoleManager = FindAnyObjectByType<ConsoleManager>();
        if (consoleManager != null)
        {
            RegisterCommands();
        }
        ApplyPermanentUpgrades();
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
        //TestUIPart();
    }

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
                case "Attack Damage":
                    attackDamage += bonus;
                    break;
                case "Projectile Speed":
                    projectileSpeed += bonus;
                    break;
                case "Duration":
                    duration += bonus;
                    break;
                case "Cooldown":
                    cooldown -= bonus; 
                    break;
                case "Size":
                    size += bonus;
                    break;
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

    public void ApplyTemporaryUpgrades()
    {
        if (playerInventory == null)
            playerInventory = FindAnyObjectByType<PlayerInventory>();
        if (playerInventory == null) return;

        foreach (var item in playerInventory.ownedItems)
        {
            if (item == null) continue;
            int level = item.GetCurrentLevel();
            if (level <= 0) continue;

            if (item is TemporaryUpgrade tempUpgrade)
            {
                float bonus = tempUpgrade.GetBonusAtLevel(level);

                switch (tempUpgrade.displayName)
                {
                    case "Attack Damage":
                        attackDamage += bonus;
                        break;
                    case "Projectile Speed":
                        projectileSpeed += bonus;
                        break;
                    case "Duration":
                        duration += bonus;
                        break;
                    case "Cooldown":
                        cooldown -= bonus;
                        break;
                    case "Size":
                        size += bonus;
                        break;
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
                continue;
            }
        }
    }


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
        }, "<value> - Gain Xp to player");
        consoleManager.RegisterCommand("enemykill", args =>
        {
            if (args[0] is string enemyType)
            {
                playerController.EnemyKilled(enemyType);
                consoleManager.AppendOutput($"Killed one enemy of type: {enemyType}");
            }
            else 
            {
                consoleManager.AppendOutput($"Invalid enemy type entered");
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
                else if(args.Length == 0)
                {
                    playerHealth.TakeDamage(20);
                    consoleManager.AppendOutput($"Player takes 20 damage.");
                }
                else
                {
                    consoleManager.AppendOutput("Invalid damage amount.");
                }
            }
        },"<value> - damage player");
    }

    // Testing purposes only
    //public int score = 0;
    //private void TestUIPart()
    //{
    //    if (Input.GetKeyDown(KeyCode.Alpha1))
    //    {
    //        playerHealth.TakeDamage(20f);
    //    }
    //    if (Input.GetKeyDown(KeyCode.Alpha2))
    //    {
    //        playerHealth.HealDamage(20f);
    //    }
    //    if (Input.GetKeyDown(KeyCode.Alpha3))
    //    {
    //        playerLevel.GainXP(50);
    //    }
    //    if (Input.GetKeyDown(KeyCode.Alpha4))
    //    {
    //        playerController.AddBuff("buff");
    //    }
    //    if (Input.GetKeyDown(KeyCode.Alpha5))
    //    {
    //        playerController.EnemyKilled("1");
    //        playerLevel.GainXP(20);
    //        score += 100;
    //    }
    //    if (Input.GetKeyDown(KeyCode.Alpha6))
    //    {
    //        playerController.EnemyKilled("2");
    //        playerLevel.GainXP(50);
    //        score += 150;
    //    }
    //    if (Input.GetKeyDown(KeyCode.Alpha7))
    //    {
    //        playerController.EnemyKilled("3");
    //        playerLevel.GainXP(100);
    //        score += 250;
    //    }
    //}
}

public enum StatType
{
    AttackDamage,
    Armor,
    MoveSpeed,
    HealthRegen,
    Experience
}