using TMPro;
using UnityEngine;

public class UpgradeDetailsView : MonoBehaviour
{
    [Header("UI Refs")]
    public PermanentUpgrade upgradeData;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text lvText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text detailsBody;
    [SerializeField] public TMP_Text miniLvText;

    void Start()
    {
        
    }

    // Update is called once per frame
    public void UpdateView()
    {
        if (upgradeData == null) return;
        int currentLevel = upgradeData.GetCurrentLevel();
        nameText.text = upgradeData.displayName;
        lvText.text = $"{currentLevel}/{upgradeData.maxLevel}";
        miniLvText.text = $"{currentLevel}";
        costText.text = currentLevel >= upgradeData.maxLevel ? "MAX" : $"{upgradeData.GetPriceForLevel(currentLevel + 1)} G";
        detailsBody.text = $"{upgradeData.description}";
    }
}
