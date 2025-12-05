using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;  

public class MainMenuController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    UIDocument _doc;


    
    //Butts 
    Button _playButton, _tutorial, _settingsButton, _creditsButton, _exitButton;
    List<Button> _buttons = new List<Button>();

    private void OnEnable()
    {
        var _doc = GetComponent<UIDocument>();
        

        _playButton = _doc.rootVisualElement.Q<Button>("PlayButton");
        _tutorial = _doc.rootVisualElement.Q<Button>("TutorialButton");
        _settingsButton = _doc.rootVisualElement.Q<Button>("SettingsButton");
        _creditsButton = _doc.rootVisualElement.Q<Button>("CreditsButton");
        _exitButton = _doc.rootVisualElement.Q<Button>("ExitButton"); // not on dument yet 

        _playButton.clicked += OnPlayClicked;
        _tutorial.clicked += OnTutorialClicked;
        _settingsButton.clicked += OnSettingsClicked;
        _creditsButton.clicked += OnCreditsClicked;
        _exitButton.clicked += OnExitClicked;

    }

    private void OnPlayClicked()
    {

        // Load the game scene or start the game

        SceneManager.LoadScene("Level1"); 

    }
    private void OnTutorialClicked()
    {
        // Load the tutorial scene or show tutorial UI
        SceneManager.LoadScene("Tutorial");
    }
    private void OnSettingsClicked()
    {
  
    }
    private void OnCreditsClicked()
    {
  
    }


    private void OnExitClicked() 
    {
        Application.Quit();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnDisable()
    {
        // good practice to unsubscribe from events
        _playButton.clicked -= OnPlayClicked;
        _tutorial.clicked -= OnTutorialClicked;
        _settingsButton.clicked -= OnSettingsClicked;
        _creditsButton.clicked -= OnCreditsClicked;
    }
}
