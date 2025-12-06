using System.Collections.Generic;
using UnityEngine;

public class TemporaryUpgradeManager : MonoBehaviour
{
    public List<TemporaryUpgrade> activeTemporaryUpgrades = new List<TemporaryUpgrade>();

    private Dictionary<string, TemporaryUpgrade> tempsById;

    private void Awake()
    {
        RebuildDictionary();
    }

    public void RebuildDictionary()
    {
        tempsById = new Dictionary<string, TemporaryUpgrade>();
        foreach (var upg in activeTemporaryUpgrades)
        {
            if (upg == null || string.IsNullOrEmpty(upg.id))
                continue;

            if (!tempsById.ContainsKey(upg.id))
                tempsById.Add(upg.id, upg);
        }
    }

    public void AddTemporaryUpgrade(TemporaryUpgrade upg)
    {
        if (upg == null) return;

        if (!activeTemporaryUpgrades.Contains(upg))
        {
            activeTemporaryUpgrades.Add(upg);
            RebuildDictionary();
        }

        upg.LevelUp(); // or your own logic when picked up
    }

    /// <summary>
    /// Returns current percent bonus (e.g. 15 means +15%) for the given temp upgrade ID.
    /// </summary>
    public float GetPercent(string tempUpgradeId)
    {
        if (tempsById == null) return 0f;

        if (tempsById.TryGetValue(tempUpgradeId, out var upg))
        {
            int lvl = upg.GetCurrentLevel();
            return upg.GetBonusAtLevel(lvl);
        }

        return 0f;
    }
}
/*when you pick a temporary upgrade in-game, just call:

TemporaryUpgradeManager.Instance.AddTemporaryUpgrade(pickedUpgrade);
PlayerStats.Instance.RecalculateFromUpgrades();*/