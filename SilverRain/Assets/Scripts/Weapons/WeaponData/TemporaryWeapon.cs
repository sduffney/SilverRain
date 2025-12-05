using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "TemporaryWeapon", menuName = "Scriptable Objects/TemporaryWeapon")]
public class TemporaryWeapon : TemporaryItem
{
    [Header("TemporaryItem Stats")]
    [SerializeField] private float baseDamage = 1;
    [SerializeField] private float baseCooldown = 5;
    [SerializeField] private float baseDuration = 1;
    [SerializeField] private float baseProjectileSpeed = 5;
    [SerializeField] private float baseSize = 1;
    [SerializeField] private float damagePerLevel = 1;
    [SerializeField] private float cooldownReductionPerLevel = 1;
    [SerializeField] private GameObject weaponPrefab;

    //Getters
    public float BaseDamage => baseDamage;
    public float BaseCooldown => baseCooldown;
    public float BaseDuration => baseDuration;
    public float BaseProjectileSpeed => baseProjectileSpeed;
    public float BaseSize => baseSize;
    public float DamagePerLevel => damagePerLevel;
    public float CooldownReductionPerLevel => cooldownReductionPerLevel;

    //Maybe move this into grenade because its specific to grenade logic
    [SerializeField] private int throwAngle;

    //Remove these when greande is removed
    public virtual void Attack() { }

    public virtual void OnActivate() { }
}
