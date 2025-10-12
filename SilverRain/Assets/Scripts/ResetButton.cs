using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResetButton : MonoBehaviour
{
    public GoldManager goldManager;
    public List<PermanentUpgrade> upgrades;
    //public UpgradeDetailsView upgradeDetailsView;
    public int currentLevel;
    private int totalRefund;

    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text lvText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text detailsBody;

    public void ResetAllProgress()
    {
        
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
    }
}
