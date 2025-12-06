using UnityEngine;

[CreateAssetMenu(fileName = "TemporaryItem", menuName = "Scriptable Objects/TemporaryItem")]
public class TemporaryItem : ScriptableObject
{
    public string id;
    public string displayName;
    public string description;
    public int currentLevel = 0;
    public int maxLevel = 5; 
    public Sprite icon;

    public int CurrentLevel {  get { return currentLevel; } set { currentLevel = value; } }

    //public virtual int GetCurrentLevel()
    //{
    //    return currentLevel;
    //}

    //public void SetCurrentLevel(int level)
    //{
    //    currentLevel = level;
    //}

    public void LevelUp()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
            //Debug.Log($"{displayName} upgraded to level {currentLevel}!");
        }
    }

    public bool isMaxLevel()
    {
        return currentLevel >= maxLevel;
    }

    public void ResetLevel()
    {
        currentLevel = 0;
    }
}