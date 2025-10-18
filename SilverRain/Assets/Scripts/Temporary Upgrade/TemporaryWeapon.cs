using UnityEngine;

[CreateAssetMenu(fileName = "TemporaryWeapon", menuName = "Scriptable Objects/TemporaryWeapon")]
public class TemporaryWeapon : TemporaryItem
{
    [Header("Weapon Stats")]
    public int currentLevel = 0;     
    public int maxLevel = 5;       
    public float baseDamage;   
    public float damagePerLevel; 
    public float baseCooldown = 1f;  
    public float cooldownReduction; 

    private float lastAttackTime = -999f;   

    public float GetDamage()
    {
        return baseDamage + (currentLevel * damagePerLevel);
    }

    public float GetCooldown()
    {
        return Mathf.Max(0.1f, baseCooldown - (currentLevel * cooldownReduction));
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void SetCurrentLevel(int level)
    {
        this.currentLevel = level;
    }

    public void LevelUp()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
            Debug.Log($"{displayName} upgraded to level {currentLevel}!");
        }
    }

    public void ResetLevel()
    {
        SetCurrentLevel(0);
        lastAttackTime = -999f;
    }
}
