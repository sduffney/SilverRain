using UnityEngine;
using UnityEngine.Events;

public class PlayerLevel : MonoBehaviour
{
    [Header("Level Settings")]
    public int playerLevel = 1;
    public float currentXP = 0f;
    public float maxXP = 100f;
    public float xpGrowthFactor = 1.5f;

    [Header("Events")]
    public UnityEvent OnLevelUp;

    private void Start()
    {
        CalculateMaxXP();
    }

    public void GainXP(float amount)
    {
        currentXP += amount;

        if (currentXP >= maxXP)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        playerLevel++;
        currentXP -= maxXP;
        CalculateMaxXP();

        OnLevelUp?.Invoke();
        

    }

    private void CalculateMaxXP()
    {
        maxXP = 100f * Mathf.Pow(xpGrowthFactor, playerLevel - 1);
    }

    public float GetXPPercentage()
    {
        return currentXP / maxXP;
    }
}