using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<Weapon> weapons = new List<Weapon>();

    public void AddWeapon(Weapon newWeapon)
    {
        if (!weapons.Contains(newWeapon))
        {
            weapons.Add(newWeapon);
            Debug.Log($"Added weapon: {newWeapon.name}");
        }
        else
        {
            Debug.Log($"Weapon {newWeapon.name} already in inventory.");
        }
    }

    public void WeaponUpgrade(Weapon weapon)
    {
        if (!weapons.Contains(weapon))
        {
            Debug.Log($"Weapon {weapon.name} not found in inventory.");
            return;
        }
        weapon.level += 1;
    }

    public void RemoveWeapon(Weapon newWeapon) {
        if (weapons.Contains(newWeapon))
        {
            weapons.Remove(newWeapon);
            Debug.Log($"Removed weapon: {newWeapon.name}");
        }
        else
        {
            Debug.Log($"Weapon {newWeapon.name} not found in inventory.");
        }
    }
}
