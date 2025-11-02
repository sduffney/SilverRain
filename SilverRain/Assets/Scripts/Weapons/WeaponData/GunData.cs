using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "Scriptable Objects/Weapons/Gun")]
public class GunData : TemporaryWeapon
{
    public override void Attack()
    {
        // Leave empty, logic is handled by GunWeaponController
    }
}