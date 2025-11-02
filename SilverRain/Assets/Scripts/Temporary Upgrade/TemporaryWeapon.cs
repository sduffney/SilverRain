using UnityEngine;

[CreateAssetMenu(fileName = "TemporaryWeapon", menuName = "Scriptable Objects/TemporaryWeapon")]
public class TemporaryWeapon : TemporaryItem
{
    [Header("TemporaryItem Stats")]      
    public float baseDamage;   
    public float damagePerLevel; 
    public float baseCooldown = 1f;  
    public float cooldownReduction; 

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
}
