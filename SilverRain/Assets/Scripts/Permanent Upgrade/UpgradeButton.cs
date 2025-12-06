using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public UpgradeDetailsView currentSelectedView;
    public GoldManager goldManager;

    [SerializeField] private Button buyButton;
    [SerializeField] private TMP_Text buyButtonText;

    private void Start()
    {
        //if (currentSelectedView == null)
        //{
        //    Debug.LogError("UpgradeDetailsView reference is missing.");
        //}

        currentSelectedView = null;
        UpdateBuyButtonState();
    }

    public void SelectUpgrades(UpgradeDetailsView clickedView)
    {
        currentSelectedView = clickedView;
        //Debug.Log($"Selected upgrade: {currentSelectedView.upgradeData.displayName}");
        UpdateBuyButtonState();
    }

    public void BuyUpgrade()
    {
        if (currentSelectedView == null)
        {
            //Debug.LogWarning("No upgrade selected.");
            return;
        }
        if (GameManager.Instance.PermanentUpgradeManager.CurrentGold >= currentSelectedView.upgradeData.currentCost)
        {
            //get current level
            int currentLevel = currentSelectedView.upgradeData.GetCurrentLevel();

            //purchase upgrade
            GameManager.Instance.PermanentUpgradeManager.RemoveGold(currentSelectedView.upgradeData.currentCost);

            //increase level
            currentLevel++;
            currentSelectedView.upgradeData.SetCurrentLevel(currentLevel);

            //update description
            string newDetail = currentSelectedView.upgradeData.GetDetailLine(currentLevel);
            currentSelectedView.upgradeData.description = BuildNaturalDescription(currentSelectedView.upgradeData.displayName, newDetail);

            currentSelectedView.UpdateView();
            //Debug.Log($"Purchased upgrade: {currentSelectedView.upgradeData.displayName}");
        }
        else
        {
            //Debug.LogWarning("Not enough gold to purchase this upgrade.");
        }
        UpdateBuyButtonState();
    }

    private string BuildNaturalDescription(string name, string detailLine)
    {
        switch (name)
        {
            case "Attack Damage":
                return $"Increases playerTrans attack power from {detailLine}.";
            case "Projectile Speed":
                return $"Increases projectile speed from {detailLine}.";
            case "Duration":
                return $"Extends skill duration from {detailLine}.";
            case "Cooldown":
                return $"Reduces skill cooldown time from {detailLine}.";
            case "Size":
                return $"Increases attack or effect size from {detailLine}.";
            case "Movement Speed":
                return $"Increases playerTrans movement speed from {detailLine}.";
            case "Health Regen":
                return $"Increases health regeneration rate from {detailLine}.";
            case "XP Amount":
                return $"Increases XP gain from {detailLine}.";
            case "Max Health":
                return $"Increases maximum health from {detailLine}.";
            case "Armour":
                return $"Increases playerTrans armour from {detailLine}.";
            default:
                return $"Improves playerTrans stats from {detailLine}.";
        }
    }

    public void UpdateBuyButtonState()
    {
        if (currentSelectedView == null)
        {
            buyButtonText.text = "BUY";
            buyButton.interactable = false;
            return;
        }

        int currentLevel = currentSelectedView.upgradeData.GetCurrentLevel();

        if (currentLevel >= currentSelectedView.upgradeData.maxLevel)
        {
            buyButtonText.text = "MAX";
            buyButton.interactable = false;
        }
        else if (currentLevel == 0)
        {
            buyButtonText.text = "BUY";
            buyButton.interactable = true;
        }
        else
        {
            buyButtonText.text = "UPGRADE";
            buyButton.interactable = true;
        }
    }
}
