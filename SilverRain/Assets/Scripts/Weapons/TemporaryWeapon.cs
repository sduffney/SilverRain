using UnityEngine;

[CreateAssetMenu(fileName = "TemporaryWeapon", menuName = "Scriptable Objects/TemporaryWeapon")]
public abstract class TemporaryWeapon : TemporaryItem
{
    [Header("TemporaryItem Stats")]
    public float baseDamage;
    public float baseCooldown = 1f;
    public float baseDuration;
    public float cooldownReduction;
    public float damagePerLevel;
    public float projectileSpeed;
    public float baseSize;      // orbit radius
    public int throwAngle;
    public PlayerStats playerStats;

    [System.NonSerialized]
    private float lastAttackTime = -999f;

    protected override void OnEnable()
    {
        // Reset runtime state
        lastAttackTime = -999f;

        // Keep weapon levels constrained
        if (maxLevel <= 0) maxLevel = 5;

        // Only search for player during play
        if (Application.isPlaying)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                playerStats = playerObj.GetComponent<PlayerStats>();
            }
            else
            {
                playerStats = null;
                Debug.LogWarning($"{name}: Player with tag 'Player' not found for TemporaryWeapon.");
            }
        }
        else
        {
            playerStats = null;
        }
    }

    public float GetDamage()
    {
        float baseVal = baseDamage + (currentLevel * damagePerLevel);

        float bonusPercent = (playerStats != null) ? playerStats.attackDamage : 0f;

        return baseVal * (1f + bonusPercent / 100f);
    }

    public float GetCooldown()
    {
        float baseCd = baseCooldown - (currentLevel * cooldownReduction);
        if (baseCd <= 0f)
        {
            Debug.LogWarning($"{name}: baseCooldown is zero or negative");
            baseCd = 0.1f;
        }

        float cdPercent = (playerStats != null) ? playerStats.cooldown : 0f; // % reduction
        float cdMult = 1f - cdPercent / 100f;
        if (cdMult < 0.1f) cdMult = 0.1f;

        return Mathf.Max(0.05f, baseCd * cdMult);
    }

    public float GetDuration()
    {
        float dur = baseDuration;
        float durPercent = (playerStats != null) ? playerStats.duration : 0f;
        return dur * (1f + durPercent / 100f);
    }

    public float GetSize()
    {
        float sizeVal = baseSize;
        float sizePercent = (playerStats != null) ? playerStats.size : 0f;
        return sizeVal * (1f + sizePercent / 100f);
    }

    public void ResetLevel()
    {
        SetCurrentLevel(0);
        lastAttackTime = -999f;
    }

    public bool IsOffCooldown()
    {
        return Time.time >= lastAttackTime + GetCooldown();
    }

    public void ResetCooldown()
    {
        lastAttackTime = Time.time;
    }

    public abstract void Attack();
}
