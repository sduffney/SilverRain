using UnityEngine;

public abstract class TempWeapon : TemporaryItem
{
    public WeaponStats stats;
    public int currentLevel = 1;

    protected float lastAttackTime;

    public bool IsOffCooldown() =>
        Time.time >= lastAttackTime + GetCooldown();

    protected void ResetCooldown() =>
        lastAttackTime = Time.time;

    public virtual float GetDamage()
    {
        // Example: +20% damage per level
        return stats.baseDamage * (1 + 0.2f * (currentLevel - 1));
    }

    public virtual float GetCooldown()
    {
        // Example: -5% cooldown per level
        return Mathf.Max(0.1f, stats.baseCooldown * (1 - 0.05f * (currentLevel - 1)));
    }

    public abstract void Attack();

    //stats
    //updateStats()
    //Attack()
    //AttackCoroutine();
    //Cooldown()
    //CoroutineAttackCooldown()
}