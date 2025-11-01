using UnityEngine;

[CreateAssetMenu(fileName = "GrenadeData", menuName = "Scriptable Objects/Weapons/Grenade")]
public class GrenadeData : TemporaryWeapon
{
    public override void Attack()
    {
        // Leave empty, logic is handled by GrenadeWeaponController
    }
}