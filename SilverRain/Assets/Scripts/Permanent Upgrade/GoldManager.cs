using TMPro;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    //public static GoldManager Instance;
    public int currentGold;
    public TMP_Text goldText;

    //private void Awake()
    //{
    //    if (Instance != null && Instance != this)
    //    {
    //        Destroy(this.gameObject);
    //    }
    //    else
    //    {
    //        Instance = this;
    //    }
    //}

    private void Start()
    {
        //PlayerPrefs.DeleteAll(); // For testing purposes only. Remove this line in production.
        currentGold = PlayerPrefs.GetInt("Gold", currentGold);  
        UpdateGoldText();
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        PlayerPrefs.SetInt("Gold", currentGold);
        UpdateGoldText();
    }

    public void RemoveGold(int amount)
    {
        currentGold -= amount;
        if (currentGold < 0) currentGold = 0;

        PlayerPrefs.SetInt("Gold", currentGold);
        UpdateGoldText();
    }

    private void UpdateGoldText()
    {
        goldText.text = $"Gold: {currentGold}G";
    }
}
