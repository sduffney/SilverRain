using System;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private int currentPlayerLevel = 1;
    [SerializeField] private float currentXP = 0f;
    [SerializeField] private float maxXP = 100f;
    [SerializeField] private float xpGrowthFactor = 1.5f;

    //Getters
    public int CurrentPlayerLevel => currentPlayerLevel;
    public float CurrentXP => currentXP;
    public float MaxXP => maxXP;

    //Events
    public static event Action OnLevelUp;

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
            //Debug.Log($"Level Up! Current Level: {currentPlayerLevel}");
        }
    }

    public bool IsLevelUp()
    {
        if (currentXP >= maxXP)
        {
            LevelUp();
            return true;
        }
        return false;
    }

    private void LevelUp()
    {
        currentPlayerLevel++;
        currentXP -= maxXP;
        CalculateMaxXP();

        OnLevelUp?.Invoke();
        //Debug.Log($"Level Up! Current Level: {playerLevel}");
    }

    private void CalculateMaxXP()
    {
        maxXP = 100f * Mathf.Pow(xpGrowthFactor, currentPlayerLevel - 1);
    }

    public float GetXPPercentage()
    {
        return currentXP / maxXP;
    }
}