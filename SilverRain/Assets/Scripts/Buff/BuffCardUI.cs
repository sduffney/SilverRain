using TMPro;
using UnityEngine;

public class BuffCardUI : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text descText;
    public TMP_Text levelText;
    private TemporaryItem data;

    private BuffManager buffManager;

    // Setup the card with data
    public void Setup(TemporaryItem itemData, BuffManager manager)
    {
        data = itemData;
        buffManager = manager;
        nameText.text = data.displayName;
        descText.text = data.description;
        if (data is TemporaryWeapon weapon)
        {
            levelText.text = $"Level: {weapon.GetCurrentLevel()}/{weapon.maxLevel}";
        }
        else if (data is TemporaryUpgrade upgrade)
        {
            levelText.text = $"Level: {upgrade.GetCurrentLevel()}/{upgrade.maxLevel}\n{upgrade.GetDetailLine(upgrade.GetCurrentLevel())}";
        }
    }

    // Called when the card is clicked
    public void OnCardClicked()
    {
        //Debug.Log("Card Click Received!");

        if (data != null && buffManager != null)
        {
            buffManager.ApplyBuff(data);
        }
    }
}
