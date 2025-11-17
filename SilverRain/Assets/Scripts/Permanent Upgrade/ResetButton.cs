using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    public GoldManager goldManager;
    public List<PermanentUpgrade> upgrades;
    //public UpgradeDetailsView upgradeDetailsView;
    public int currentLevel;
    private int totalRefund;
    public UpgradeButton upgradeButton;

    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text lvText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text detailsBody;
    [SerializeField] private TMP_Text[] miniLvs;

    private void Start()
    {
        SyncMiniLevels();
    }

    public void ResetAllProgress()
    {
        totalRefund = 0;

        foreach (var upgrade in upgrades)
        {
            //calculate total refund
            if (upgrade != null)
            {
                for (int level = 1; level <= upgrade.GetCurrentLevel(); level++)
                {
                    totalRefund += upgrade.GetPriceForLevel(level);
                }
                upgrade.SetCurrentLevel(0);
            }
        }

        goldManager.AddGold(totalRefund);

        // Clear upgrade details view
        ClearView();
        Debug.Log("All progress has been reset.");
    }

    public void ClearView()
    {
        nameText.text = "Name";
        lvText.text = "Lv";
        costText.text = "Cost";
        detailsBody.text = "";

        upgradeButton.currentSelectedView = null;
        upgradeButton.UpdateBuyButtonState();

        foreach (var miniLv in miniLvs)
        {
            miniLv.text = "0";
        }
    }

    public void SyncMiniLevels()
    {
        for (int i = 0; i < upgrades.Count; i++)
        {
            if (upgrades[i] != null)
            {
                int lv = upgrades[i].GetCurrentLevel();
                miniLvs[i].text = lv.ToString();
            }
            else
            {
                miniLvs[i].text = "0";
            }
        }
    }

}
