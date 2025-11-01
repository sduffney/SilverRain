using UnityEngine;

[CreateAssetMenu(fileName = "SwordData", menuName = "Scriptable Objects/Weapons/Sword")]
public class SwordData : TemporaryWeapon
{
    public override void Attack()
    {
        // ScriptableObjects don’t run gameplay logic directly.
        // Leave for debugging.
    }
}