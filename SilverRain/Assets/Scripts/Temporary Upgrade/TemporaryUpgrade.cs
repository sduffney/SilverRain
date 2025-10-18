using UnityEngine;

[CreateAssetMenu(fileName = "TemporaryUpgrade", menuName = "Scriptable Objects/TemporaryUpgrade")]
public class TemporaryUpgrade : TemporaryItem
{
    public string id;                 // unique key
    public string bonusUnit;
    public int maxLevel = 10;
    public string cachedDetailLine;

    [Header("Pricing")]
    public int baseCost = 100;
    public float costAdder = 100f;
    public int currentCost;

    [Header("Bonus")]
    public float baseBonus = 5f;
    public float bonusPerLevel = 5f;   // increment per level
    private int currentLevel = 0;

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
        return currentLevel;
    }

    //set current level
    public void SetCurrentLevel(int level)
    {
       this.currentLevel = level;
    }

    public string GetDetailLine(int currentLevel)
    {
        float cur = currentLevel <= 0 ? 0f : GetBonusAtLevel(currentLevel);
        float next = currentLevel >= maxLevel ? cur : GetBonusAtLevel(currentLevel + 1);
        cachedDetailLine = $"{cur}  →  {next}{bonusUnit}";
        return cachedDetailLine;
    }

    public void ResetLevel()
    {
        SetCurrentLevel(0);
    }
}
