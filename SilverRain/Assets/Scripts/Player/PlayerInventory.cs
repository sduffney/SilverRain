using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<TemporaryItem> ownedItems = new List<TemporaryItem>();
    public BuffManager buffManager;

    [Header("Weapon Controllers on Player")]
    public GunWeaponController gunController;
    public SwordWeaponController swordController;
    public GrenadeWeaponController grenadeController;

    [Header("Weapon Data Assets (ScriptableObjects)")]
    // These can be SwordData / GrenadeData too, as long as they inherit from TemporaryWeapon
    public TemporaryWeapon gunData;
    public TemporaryWeapon swordData;
    public TemporaryWeapon grenadeData;

    private void Update()
    {
        // Check levels EVERY FRAME and keep weapons in sync
        SyncWeaponWithLevel(gunData, gunController);
        SyncWeaponWithLevel(swordData, swordController);
        SyncWeaponWithLevel(grenadeData, grenadeController);
    }

    private void SyncWeaponWithLevel(TemporaryWeapon weaponData, WeaponController controller)
    {
        if (weaponData == null || controller == null)
            return;

        // “Active” means level > 0 (you said > 1 but logically you unlock at 1;
        // change to > 1 if you really want it to start at level 2)
        bool shouldBeActive = weaponData.GetCurrentLevel() > 0;
        bool isActive = controller.gameObject.activeSelf;

        // Turn ON + call OnActivate once when we cross from inactive → active
        if (shouldBeActive && !isActive)
        {
            controller.gameObject.SetActive(true);
            controller.OnActivate();
        }
        // Turn OFF when we drop back to level 0 (or remove the weapon)
        else if (!shouldBeActive && isActive)
        {
            controller.gameObject.SetActive(false);
        }
    }

    public void PickItem(TemporaryItem newItem)
    {
        if (!ownedItems.Contains(newItem))
        {
            ownedItems.Add(newItem);
            newItem.SetCurrentLevel(1);
        }
        else
        {
            ItemUpgrade(newItem);
        }
    }

    private void ItemUpgrade(TemporaryItem item)
    {
        if (!ownedItems.Contains(item))
        {
            return;
        }

        item.LevelUp();
    }

    public void ClearAllItems()
    {
        ownedItems.Clear();
    }
}
