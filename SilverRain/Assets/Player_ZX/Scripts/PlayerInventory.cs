using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<TemporaryItem> tempItems = new List<TemporaryItem>();

    public void PickItem(TemporaryItem newItem)
    {
        if (!tempItems.Contains(newItem))
        {
            tempItems.Add(newItem);
        }
        else
        {
            ItemUpgrade(newItem);
        }

    }

    private void ItemUpgrade(TemporaryItem item)
    {
        if (!tempItems.Contains(item))
        {
            return;
        }
        item.LevelUp();
    }

    public void ClearAllItems()
    {
        tempItems.Clear();
    }
}
