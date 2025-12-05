using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PermanentUpgradeManager : MonoBehaviour
{
    public static PermanentUpgradeManager Instance { get; private set; }
    public List<PermanentUpgrade> allPermanentUpgrades;

    private Dictionary<string, PermanentUpgrade> upgradesById;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

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
}
