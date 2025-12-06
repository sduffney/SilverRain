using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private List<TemporaryItem> ownedItems = new List<TemporaryItem>();
    [SerializeField] private BuffManager buffManager;

    public List<TemporaryItem> OwnedItems => ownedItems;

    //Initial Weapon
    [SerializeField] private TemporaryWeapon initialWeapon;

    [Header("Weapon Controllers")]
    [SerializeField] private GunWeaponController gunController;
    [SerializeField] private SwordWeaponController swordController;
    [SerializeField] private GrenadeWeaponController grenadeController;

    //TemporaryWeapon is a scriptable object that holds weapon data
    [Header("Weapon Data")]
    [SerializeField] private TemporaryWeapon gunData;
    [SerializeField] private TemporaryWeapon swordData;
    [SerializeField] private TemporaryWeapon grenadeData;

    private void Start()
    {
        if (initialWeapon != null)
        {
            PickItem(initialWeapon);
        }
    }
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
        bool shouldBeActive = weaponData.CurrentLevel > 0;
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
            newItem.CurrentLevel = 1;
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
