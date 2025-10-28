using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public List<TemporaryItem> allTempItems;
    public GameObject buffCardPrefab;
    public Transform cardParent;

    //when player levels up, show 3 random buffs to choose from
    public void ShowBuffOptions()
    {
        // Clear existing cards
        foreach (Transform child in cardParent)
        {
            Destroy(child.gameObject);
        }

        // Shuffle and pick 3 unique buffs
        List<TemporaryItem> options = new List<TemporaryItem>();
        List<TemporaryItem> combinedPool = allTempItems.FindAll(item => !item.isMaxLevel());

        System.Random rand = new System.Random();
        while (options.Count < 3 && combinedPool.Count > 0)
        {
            int index = rand.Next(combinedPool.Count);
            options.Add(combinedPool[index]);
            combinedPool.RemoveAt(index);
        }

        // Instantiate buff cards
        foreach (var item in options)
        {
            GameObject card = Instantiate(buffCardPrefab, cardParent);
            BuffCardUI buffCard = card.GetComponent<BuffCardUI>();
            if (buffCard != null)
            {
                buffCard.Setup(item, this);
            }
        }
    }
       
    public void ApplyBuff(TemporaryItem item)
    {
        item.LevelUp();
        Debug.Log($"Applied buff: {item.displayName} to level {item.GetCurrentLevel()}");
    }
}
