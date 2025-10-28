using System;
using UnityEngine;

public class PlayerLevelSystem : MonoBehaviour
{
    public int currentLevel;
    public int currentXP;
    public int xpToNextLevel;
    public BuffManager buffManager;

    private void Start()
    {
        currentLevel = 1;
        currentXP = 0;
        xpToNextLevel = 100; 
    }

    public void AddXP(int amount)
    {
        currentXP += amount;
        if (currentXP >= xpToNextLevel)
        {
            currentLevel++;
            currentXP -= xpToNextLevel;
            buffManager.ShowBuffOptions();
        }
    }
}
