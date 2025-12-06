using System.Collections;
using UnityEngine;

public abstract class WeaponController : MonoBehaviour
{
    [SerializeField] protected TemporaryWeapon weaponData;
    public abstract void OnActivate();
    public abstract IEnumerator OnDuration();
    public abstract IEnumerator OnCoolDown();
    public abstract void Attack();

    public float GetDamage()
    {
        float baseVal = weaponData.BaseDamage + (weaponData.currentLevel * weaponData.DamagePerLevel);
        float bonusPercent = GameManager.Instance.PlayerStats.attackDamage;
        return baseVal * (1f + bonusPercent / 100f);
    }

    public float GetCooldown()
    {
        float baseCd = weaponData.BaseCooldown - (weaponData.currentLevel * weaponData.CooldownReductionPerLevel);
        if (baseCd <= 0f)
        {
            Debug.LogWarning($"{name}: baseCooldown is zero or negative");
            baseCd = 1f;
        }

        float cdPercent = GameManager.Instance.PlayerStats.cooldown; // % reduction
        float cdMult = 1f - cdPercent / 100f;
        if (cdMult < 0.1f) cdMult = 0.1f;

        return Mathf.Max(0.05f, baseCd * cdMult);
    }

    public float GetDuration()
    {
        float dur = weaponData.BaseDuration;
        float durPercent = GameManager.Instance.PlayerStats.duration;
        return dur * (1f + durPercent / 100f);
    }

    public float GetSize()
    {
        if (GameManager.Instance == null || GameManager.Instance.PlayerStats == null)
        {
            Debug.LogWarning("GameManager or PlayerStats not initialized yet.");
            return weaponData.BaseSize; // fallback to base size
        }

        float sizeVal = weaponData.BaseSize;
        float sizePercent = GameManager.Instance.PlayerStats.size;
        return sizeVal * (1f + sizePercent / 100f);
    }

    public void ResetLevel()
    {
        weaponData.ResetLevel();
    }

    private void OnDestroy()
    {
        weaponData.ResetLevel();
    }
}