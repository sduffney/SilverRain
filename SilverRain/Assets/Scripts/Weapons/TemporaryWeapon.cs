using Unity.VisualScripting.FullSerializer;
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

    // Runtime-only state (do not serialize to asset)
    [System.NonSerialized]
    private float lastAttackTime = -999f;

    // If currentLevel is modified at runtime and you don't want to persist it, mark and reset it
    // [System.NonSerialized]
    // private int runtimeCurrentLevel = 0;

    protected override void OnEnable()
    {
        // Keep config values as-is (they come from the asset).
        maxLevel = 5;

        // Reset runtime-only state every time the asset is reloaded/enabled
        lastAttackTime = -999f;
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();

        // If you are using a runtime-only currentLevel, reset it here:
        // runtimeCurrentLevel = 0;

        // If TemporaryItem has a currentLevel stored on the asset and you want it reset between plays:
        // SetCurrentLevel(0);
    }

    public float GetDamage()
    {
        return baseDamage + (currentLevel * damagePerLevel) + playerStats.attackDamage;
    }

    public float GetCooldown()
    {
        if (baseCooldown <= 0f) Debug.LogWarning($"{name}: baseCooldown is zero or negative");
        return Mathf.Max(0.1f, baseCooldown - (currentLevel * cooldownReduction) - playerStats.cooldown);
    }

    public void ResetLevel()
    {
        SetCurrentLevel(0);
        lastAttackTime = -999f;
    }

    public bool IsOffCooldown()
    {
        Debug.Log(Time.time + " >= " + lastAttackTime + " + " + GetCooldown());
        return Time.time >= lastAttackTime + GetCooldown();
    }

    public void ResetCooldown()
    {
        lastAttackTime = Time.time;
    }

    public abstract void Attack();
}
