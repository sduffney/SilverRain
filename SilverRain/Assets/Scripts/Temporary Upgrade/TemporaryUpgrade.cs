using UnityEngine;

[CreateAssetMenu(fileName = "TemporaryUpgrade", menuName = "Scriptable Objects/TemporaryUpgrade")]
public class TemporaryUpgrade : TemporaryItem
{
    public string bonusUnit;
    public string cachedDetailLine;

    [Header("Bonus")]
    public float baseBonus = 5f;
    public float bonusPerLevel = 5f;  

    public float GetBonusAtLevel(int level)
    {
        // total bonus at current level
        return baseBonus + Mathf.Max(0, level - 1) * bonusPerLevel;
    }

    public string GetDetailLine(int currentLevel)
    {
        float cur = currentLevel <= 0 ? 0f : GetBonusAtLevel(currentLevel);
        float next = currentLevel >= maxLevel ? cur : GetBonusAtLevel(currentLevel + 1);
        cachedDetailLine = $"{cur}  →  {next}{bonusUnit}";
        return cachedDetailLine;
    }

    private void OnDestroy()
    {
        ResetLevel();
    }
}
