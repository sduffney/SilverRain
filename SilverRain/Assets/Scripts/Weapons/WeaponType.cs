using UnityEngine;

public enum WeaponType { Sword, Gun, Grenade }

[CreateAssetMenu(menuName = "SilverRain/WeaponStats")]
public class WeaponStats : ScriptableObject
{
    public string displayName;
    public WeaponType type;

    [Header("Core")]
    public float baseDamage;
    public float baseCooldown;
    public int maxLevel = 5;

    [Header("Ranged/Grenade")]
    public float projectileSpeed;
    public float aoeRadius;

    [Header("Melee")]
    public float reach;
    public float arcDegrees;

    [Header("FX")]
    public AudioClip fireSfx;
    public AudioClip hitSfx;
    public GameObject muzzleVfx;
    public GameObject hitVfx;
}
