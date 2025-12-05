using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class BuffManager : MonoBehaviour
{
    public List<TemporaryItem> allTempItems; // add three weapons
    public GameObject buffCardPrefab;
    public Transform cardParent;

    private GameObject player;
    private PlayerInventory playerInventory;
    private PlayerController playerController;
    private PlayerLevel playerLevel;

    void Start()
    {
        player = GameManager.Instance.Player;
        playerInventory = player.GetComponent<PlayerInventory>();
        playerController = player.GetComponent<PlayerController>();
        playerLevel = player.GetComponent<PlayerLevel>();
        //ResetBuff(allTempItems);
        SyncBuffLevelsWithInventory();

        //var defaultWeapon = allTempItems.Find(item => item.id == "sword");
        //playerInventory.PickItem(defaultWeapon);

        //Subscribe to events
        PlayerLevel.OnLevelUp += ShowBuffOptions;
    }

    //Unsubscribe from events
    private void OnDisable() { PlayerLevel.OnLevelUp -= ShowBuffOptions; }
    private void OnDestroy() { PlayerLevel.OnLevelUp -= ShowBuffOptions; }

    //when playerTrans levels up, show 3 random buffs to choose from
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
        //playerController.FreezePlayer();
        GameManager.Instance.RequestPause();


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
        //Debug.Log($"Applied buff: {item.displayName} to level {item.GetCurrentLevel()}");

        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.ApplyTemporaryUpgrades();
        }

        

        cardParent.gameObject.SetActive(false);
        //if (playerLevel.IsLevelUp()) ShowBuffOptions();
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        //playerController.UnfreezePlayer();
        GameManager.Instance.ReleasePause();
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
        // if not in the Level scene, skip
        if (SceneManager.GetActiveScene().name != "Level1")
            return;

        if (playerInventory == null)
            playerInventory = FindAnyObjectByType<PlayerInventory>();

        foreach (var buff in allTempItems)
        {
            if (buff == null) continue;
            TemporaryItem owned = playerInventory.ownedItems.Find(item => item.id == buff.id);
            if (owned != null)
            {
                buff.SetCurrentLevel(owned.GetCurrentLevel());
                //Debug.Log($"Sync: {buff.displayName} set to level {buff.GetCurrentLevel()}");
            }
        }
    }

}
