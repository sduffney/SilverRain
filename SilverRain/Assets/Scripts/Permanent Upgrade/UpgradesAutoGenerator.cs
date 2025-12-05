#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public static class UpgradesAutoGenerator
{
    private class Preset
    {
        public string id;
        public string displayName;
        public string bonusUnit;
        public int maxLevel;
        public int baseCost;
        public float costAdder;
        public float baseBonus;
        public float bonusPerLevel;

        public Preset(string id, string name, string unit, int maxLvl,
                      int baseCost, float costAdder, float baseBonus, float perLvl)
        {
            this.id = id;
            this.displayName = name;
            this.bonusUnit = unit;
            this.maxLevel = maxLvl;
            this.baseCost = baseCost;
            this.costAdder = costAdder;
            this.baseBonus = baseBonus;
            this.bonusPerLevel = perLvl;
        }
    }

    private static readonly Preset[] PRESETS = new Preset[]
    {
        new Preset("upgrade_attackDamage",   "Attack Damage",   "%",  10, 100, 100f, 10f, 5f),
        new Preset("upgrade_projectileSpd",  "Projectile Speed","%",       10, 100,  90f, 10f, 5f),
        new Preset("upgrade_duration",       "Duration",        "%",       10, 120, 100f, 10f, 5f),
        new Preset("upgrade_cooldown",       "Cooldown",        "%",       10, 150, 120f,  5f, 3f),
        new Preset("upgrade_size",           "Size",            "%",       10, 120, 100f, 10f, 5f),
        new Preset("upgrade_moveSpeed",      "Movement Speed",  "%",       10, 120, 100f,  5f, 3f),
        new Preset("upgrade_healthRegen",    "Health Regen",    "HP/s",    10, 150, 120f, 0.5f,0.5f),
        new Preset("upgrade_xpAmount",       "XP Amount",       "%",       10, 100, 100f, 10f,10f),
        new Preset("upgrade_maxHealth",      "Max Health",      "HP",      10, 150, 120f, 10f,10f),
        new Preset("upgrade_armour",         "Armour",          "%",       10, 150, 120f,  5f, 3f)
    };

    [MenuItem("Tools/Silver Rain/Generate Permanent Upgrades")]
    public static void Generate()
    {
        string folder = "Assets/Upgrades";
        if (!AssetDatabase.IsValidFolder(folder))
        {
            Directory.CreateDirectory(folder);
            AssetDatabase.Refresh();
        }

        foreach (var p in PRESETS)
        {
            string assetPath = $"{folder}/{p.displayName}.asset";
            var so = AssetDatabase.LoadAssetAtPath<PermanentUpgrade>(assetPath);

            if (so == null)
                so = ScriptableObject.CreateInstance<PermanentUpgrade>();

            so.id = p.id;
            so.displayName = p.displayName;
            so.bonusUnit = p.bonusUnit;
            so.maxLevel = p.maxLevel;
            so.baseCost = p.baseCost;
            so.costAdder = p.costAdder;
            so.baseBonus = p.baseBonus;
            so.bonusPerLevel = p.bonusPerLevel;

            // Generate detail line: 0 → first level bonus
            float next = so.GetBonusAtLevel(1);
            so.cachedDetailLine = $"0{so.bonusUnit} → {next}{so.bonusUnit}";

            // Generate natural English description
            so.description = BuildNaturalDescription(so.displayName, so.cachedDetailLine);

            if (AssetDatabase.Contains(so))
                EditorUtility.SetDirty(so);
            else
                AssetDatabase.CreateAsset(so, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[Upgrade Generator] All PermanentUpgrade assets generated with English descriptions.");
    }

    private static string BuildNaturalDescription(string name, string detailLine)
    {
        switch (name)
        {
            case "Attack Damage":
                return $"Increases playerTrans attack power from {detailLine}.";
            case "Projectile Speed":
                return $"Increases projectile speed from {detailLine}.";
            case "Duration":
                return $"Extends skill duration from {detailLine}.";
            case "Cooldown":
                return $"Reduces skill cooldown time from {detailLine}.";
            case "Size":
                return $"Increases attack or effect size from {detailLine}.";
            case "Movement Speed":
                return $"Increases playerTrans movement speed from {detailLine}.";
            case "Health Regen":
                return $"Increases health regeneration rate from {detailLine}.";
            case "XP Amount":
                return $"Increases XP gain from {detailLine}.";
            case "Max Health":
                return $"Increases maximum health from {detailLine}.";
            case "Armour":
                return $"Increases playerTrans armour from {detailLine}.";
            default:
                return $"Improves playerTrans stats from {detailLine}.";
        }
    }
}
#endif
