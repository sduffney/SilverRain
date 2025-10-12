using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour
{
    public LevelSelector levelSelector;
    public void GoToLevelSelector()
    {
        SceneManager.LoadScene("LevelSelector");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToSettings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void GoToGame()
    {
        if (levelSelector.selectedLevel == -1)
        {
            Debug.LogWarning("No level selected. Cannot start the game.");
            return;
        }
        else
        {
            SceneManager.LoadScene($"Level{levelSelector.selectedLevel + 1}");
        }
    }
}
