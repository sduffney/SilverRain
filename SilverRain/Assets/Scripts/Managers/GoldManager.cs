using TMPro;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    //public static GoldManager Instance;
    [SerializeField] private TMP_Text goldText;

    private void Start()
    {
        UpdateGoldText();
        //Subscribe to event
        PermanentUpgradeManager.OnGoldChange += UpdateGoldText;
    }

    //Unsubscribe from event
    private void OnDestroy() { PermanentUpgradeManager.OnGoldChange -= UpdateGoldText; }

    private void OnDisable() { PermanentUpgradeManager.OnGoldChange -= UpdateGoldText; }

    private void UpdateGoldText()
    {
        goldText.text = $"Gold: {GameManager.Instance.PermanentUpgradeManager.CurrentGold}";
    }
}