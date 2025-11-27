using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public Button[] levelButtons;
    public int selectedLevel;
    public Button startButton;

    public EventSystem eventSystem;

    public void Start()
    {
        selectedLevel = -1; // No level selected initially
        //UpdateStartButtonState();
    }

    public void SelectLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levelButtons.Length)
        {
            Debug.LogWarning("Invalid level index selected.");
            return;
        }
        selectedLevel = levelIndex;
        eventSystem.SetSelectedGameObject(levelButtons[levelIndex].gameObject);
        Debug.Log($"Selected level: {selectedLevel + 1}");
        //UpdateStartButtonState();
    }

}
