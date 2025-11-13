#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public static class TemporaryUpgradeAutoGenerator
{
    private class Preset
    {
        public string id;
        public string displayName;
        public string bonusUnit;
        public int maxLevel;
        public float baseBonus;
        public float bonusPerLevel;

        public Preset(string id, string name, string unit, int maxLvl,
                      float baseBonus, float perLvl)
        {
            this.id = id;
            this.displayName = name;
            this.bonusUnit = unit;
            this.maxLevel = maxLvl;
            this.baseBonus = baseBonus;
            this.bonusPerLevel = perLvl;
        }
    }

    private static readonly Preset[] PRESETS = new Preset[]
    {
        new Preset("temp_attackDamage",   "Attack Damage",   "%", 5, 10f, 5f),
        new Preset("temp_projectileSpd",  "Projectile Speed","%", 5, 10f, 5f),
        new Preset("temp_duration",       "Duration",        "%", 5, 10f, 5f),
        new Preset("temp_cooldown",       "Cooldown",        "%", 5,  5f, 3f),
        new Preset("temp_size",           "Size",            "%", 5, 10f, 5f),
        new Preset("temp_moveSpeed",      "Movement Speed",  "%", 5,  5f, 3f),
        new Preset("temp_healthRegen",    "Health Regen",    "HP/s", 5, 0.5f, 0.5f),
        new Preset("temp_xpAmount",       "XP Amount",       "%", 5, 10f, 10f),
        new Preset("temp_maxHealth",      "Max Health",      "HP", 5, 10f, 10f),
        new Preset("temp_armour",         "Armour",          "%", 5, 5f, 3f)
    };

    [MenuItem("Tools/Silver Rain/Generate Temporary Upgrades")]
    public static void Generate()
    {
        string folder = "Assets/TemporaryUpgrades";
        if (!AssetDatabase.IsValidFolder(folder))
        {
            Directory.CreateDirectory(folder);
            AssetDatabase.Refresh();
        }

        foreach (var p in PRESETS)
        {
            string assetPath = $"{folder}/{p.displayName}.asset";
            var so = AssetDatabase.LoadAssetAtPath<TemporaryUpgrade>(assetPath);

            if (so == null)
                so = ScriptableObject.CreateInstance<TemporaryUpgrade>();

            so.id = p.id;
            so.displayName = p.displayName;
            so.bonusUnit = p.bonusUnit;
            so.maxLevel = p.maxLevel;
            so.baseBonus = p.baseBonus;
            so.bonusPerLevel = p.bonusPerLevel;

            float next = so.GetBonusAtLevel(1);
            so.cachedDetailLine = $"0{so.bonusUnit} → {next}{so.bonusUnit}";
            so.description = BuildNaturalDescription(so.displayName, so.cachedDetailLine);

            if (AssetDatabase.Contains(so))
                EditorUtility.SetDirty(so);
            else
                AssetDatabase.CreateAsset(so, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[Temporary Upgrade Generator] All TemporaryUpgrade assets generated successfully.");
    }

    private static string BuildNaturalDescription(string name, string detailLine)
    {
        switch (name)
        {
            case "Attack Damage":
                return $"Temporarily increases player attack power from {detailLine}.";
            case "Projectile Speed":
                return $"Temporarily increases projectile speed from {detailLine}.";
            case "Duration":
                return $"Temporarily extends skill duration from {detailLine}.";
            case "Cooldown":
                return $"Temporarily reduces skill cooldown time from {detailLine}.";
            case "Size":
                return $"Temporarily increases attack or effect size from {detailLine}.";
            case "Movement Speed":
                return $"Temporarily increases player movement speed from {detailLine}.";
            case "Health Regen":
                return $"Temporarily increases health regeneration rate from {detailLine}.";
            case "XP Amount":
                return $"Temporarily increases XP gain from {detailLine}.";
            case "Max Health":
                return $"Temporarily increases maximum health from {detailLine}.";
            case "Armour":
                return $"Temporarily increases player armour from {detailLine}.";
            default:
                return $"Temporarily improves player stats from {detailLine}.";
        }
    }
}
#endif
