using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BuffManager : MonoBehaviour
{
    public List<TemporaryItem> allTempItems;
    public GameObject buffCardPrefab;
    public Transform cardParent;

    private GameObject player;
    private PlayerInventory playerInventory;

    void Start()
    {
        //Subscribe to events
        PlayerLevel.OnLevelUp += ShowBuffOptions;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //Unsubscribe from events
    private void OnDisable()
    {
        PlayerLevel.OnLevelUp -= ShowBuffOptions;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnDestroy()
    {
        PlayerLevel.OnLevelUp -= ShowBuffOptions;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Get components
        player = GameManager.Instance.Player;
        if (player != null)
        {
            playerInventory = player.GetComponent<PlayerInventory>();
        }
        if (playerInventory != null)
        {
            //ResetBuff(allTempItems);
            SyncBuffLevelsWithInventory();
        }
    }

    //when playerTrans levels up, show 3 random buffs to choose from
    public void ShowBuffOptions()
    {
        EventSystem.current.SetSelectedGameObject(null);
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