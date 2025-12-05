using UnityEngine;

[CreateAssetMenu(fileName = "SwordData", menuName = "Scriptable Objects/Weapons/Sword")]
public class SwordData : TemporaryWeapon
{
    public override void Attack()
    {
        // ScriptableObjects hold stats; gameplay runs in MonoBehaviours.
    }

    public override void OnActivate()
    {
        throw new System.NotImplementedException();
    }
}