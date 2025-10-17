using TMPro;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance;
    public int currentGold;
    public TMP_Text goldText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        UpdateGoldText();
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        UpdateGoldText();
    }

    public void RemoveGold(int amount) {
        currentGold -= amount;
        if (currentGold < 0) currentGold = 0;
        UpdateGoldText();
    }

    private void UpdateGoldText()
    {
        goldText.text = $"Gold: {currentGold}G";
    }
}
