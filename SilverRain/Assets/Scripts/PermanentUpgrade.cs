using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Permanent Upgrade")]
public class PermanentUpgrade : ScriptableObject
{
    public string id;                 // unique key
    public string displayName;        
    public string bonusUnit;          // e.g. "% dmg", "HP", "movespeed"
    public int maxLevel = 10;
    public string description;
    public string cachedDetailLine;

    [Header("Pricing")]
    public int baseCost = 100;
    public float costAdder = 100f;        // e.g. +50 each level 
    public int currentCost;

    [Header("Bonus")]
    public float baseBonus = 5f;       // e.g. +5%
    public float bonusPerLevel = 5f;   // increment per level

    public int GetPriceForLevel(int level)
    {
        // next level price 
        currentCost = Mathf.RoundToInt(baseCost + (level - 1) * costAdder);
        return currentCost;
    }

    public float GetBonusAtLevel(int level)
    {
        // total bonus at current level
        return baseBonus + Mathf.Max(0, level - 1) * bonusPerLevel;
    }

    //get current level
    public int GetCurrentLevel()
    {
        return PlayerPrefs.GetInt(id, 0);
    }

    //set current level
    public void SetCurrentLevel(int level)
    {
        PlayerPrefs.SetInt(id, level);
    }

    public string GetDetailLine(int currentLevel)
    {
        float cur = currentLevel <= 0 ? 0f : GetBonusAtLevel(currentLevel);
        float next = currentLevel >= maxLevel ? cur : GetBonusAtLevel(currentLevel + 1);
        cachedDetailLine = $"{cur}  →  {next}{bonusUnit}";
        return cachedDetailLine;
    }
}
