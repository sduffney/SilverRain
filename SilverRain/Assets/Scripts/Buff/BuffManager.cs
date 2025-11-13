using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class BuffManager : MonoBehaviour
{
    public List<TemporaryItem> allTempItems;
    public GameObject buffCardPrefab;
    public Transform cardParent;
    
    private PlayerInventory playerInventory;
    private PlayerController playerController;
    private PlayerLevel playerLevel;

    void Start()
    {
        playerInventory = FindAnyObjectByType<PlayerInventory>();
        playerController = FindAnyObjectByType<PlayerController>();
        playerLevel = FindAnyObjectByType<PlayerLevel>();
        //ResetBuff(allTempItems);
        SyncBuffLevelsWithInventory();
    }

    //when player levels up, show 3 random buffs to choose from
    public void ShowBuffOptions()
    {
        //var playerController = FindAnyObjectByType<PlayerController>();
        //if (playerController != null)
        //{
        //    playerController.UnfreezePlayer(); 
        //    Debug.Log("Unfreezing Player for Buff Selection");
        //}

        //var playerInput = GameObject.FindAnyObjectByType<PlayerInput>();
        //if (playerInput != null)
        //{
        //    playerInput.enabled = true;
        //}

        EventSystem.current.SetSelectedGameObject(null);
        //Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;
        playerController.FreezePlayer();


        cardParent.gameObject.SetActive(true);
        
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
        playerInventory.PickItem(item);
        Debug.Log($"Applied buff: {item.displayName} to level {item.GetCurrentLevel()}");

        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.ApplyTemporaryUpgrades();
        }

        

        cardParent.gameObject.SetActive(false);
        if (playerLevel.IsLevelUp()) ShowBuffOptions();
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        playerController.UnfreezePlayer();
    }

    public void ResetBuff(List<TemporaryItem> allTempItems)
    {
         foreach (var item in allTempItems)
         {
             item.ResetLevel();
        }
    }

    public void SyncBuffLevelsWithInventory()
    {
        if (playerInventory == null)
            playerInventory = FindAnyObjectByType<PlayerInventory>();

        foreach (var buff in allTempItems)
        {
            if (buff == null) continue;
            TemporaryItem owned = playerInventory.ownedItems.Find(item => item.id == buff.id);
            if (owned != null)
            {
                buff.SetCurrentLevel(owned.GetCurrentLevel());
                Debug.Log($"Sync: {buff.displayName} set to level {buff.GetCurrentLevel()}");
            }
        }
    }

}
