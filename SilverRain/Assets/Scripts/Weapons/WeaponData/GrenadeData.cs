using UnityEngine;

[CreateAssetMenu(fileName = "GrenadeData", menuName = "Scriptable Objects/Weapons/Grenade")]
public class GrenadeData : TemporaryWeapon
{
    [Header("Grenade Settings")]
    public float throwForce = 12f;
    public float upwardForce = 4f;

    public override void Attack()
    {
        // Leave empty, logic is handled by GrenadeWeaponController
    }
}
