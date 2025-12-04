using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<TemporaryItem> ownedItems = new List<TemporaryItem>();
    public BuffManager buffManager;

    public void PickItem(TemporaryItem newItem)
    {
        if (!ownedItems.Contains(newItem))
        {
            ownedItems.Add(newItem);
            newItem.SetCurrentLevel(1);

            if (newItem is TemporaryWeapon temporaryWeapon)
            {
                temporaryWeapon.OnActivate();
            }
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
