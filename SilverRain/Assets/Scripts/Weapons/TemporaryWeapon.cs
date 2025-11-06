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

    private float lastAttackTime = -999f;

    protected override void OnEnable()
    {
        maxLevel = 5;
    }

    public float GetDamage()
    {
        return baseDamage + (currentLevel * damagePerLevel);
    }

    public float GetCooldown()
    {
        return Mathf.Max(0.1f, baseCooldown - (currentLevel * cooldownReduction));
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

    //stats
    //updateStats()
    //Attack()
    //AttackCoroutine();
    //Cooldown()
    //CoroutineAttackCooldown()
}
