using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class PermanentUpgradeManager : MonoBehaviour
{
    [Header("Upgrade List")]
    [SerializeField] private List<PermanentUpgrade> allPermanentUpgrades;
    private Dictionary<string, PermanentUpgrade> upgradesById;

    [Header("Gold")]
    [SerializeField] private int currentGold;
    public static event Action OnGoldChange;

    public int CurrentGold => currentGold;

    private void Awake()
    {
        upgradesById = new Dictionary<string, PermanentUpgrade>();

        if (allPermanentUpgrades != null)
        {
            foreach (var upg in allPermanentUpgrades)
            {
                if (upg == null || string.IsNullOrEmpty(upg.id))
                    continue;

                if (!upgradesById.ContainsKey(upg.id))
                    upgradesById.Add(upg.id, upg);
            }
        }

        currentGold = PlayerPrefs.GetInt("Gold", 0);
    }

    /// <summary>
    /// Returns the current percent bonus (e.g. 25 means +25%) for the given upgrade ID.
    /// </summary>
    public float GetPercent(string upgradeId)
    {
        if (upgradesById == null) return 0f;

        if (upgradesById.TryGetValue(upgradeId, out var upg))
        {
            int lvl = upg.GetCurrentLevel();
            return upg.GetBonusAtLevel(lvl);   // Your PermanentUpgrade already supports this
        }

        return 0f;
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        PlayerPrefs.SetInt("Gold", currentGold);
        OnGoldChange?.Invoke();
    }

    public void RemoveGold(int amount)
    {
        currentGold -= amount;
        currentGold = Mathf.Max(0, currentGold);
        PlayerPrefs.SetInt("Gold", currentGold);
        OnGoldChange?.Invoke();
    }
}