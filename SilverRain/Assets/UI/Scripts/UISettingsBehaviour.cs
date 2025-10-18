using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISettingsBehaviour : MonoBehaviour
{
    [SerializeField]
    private Slider volumeSlider;
    [SerializeField]
    private Toggle streamerOverlayToggle;

    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject pasueMenu;

    private GameObject creditsButton;

    private const string CREDITS_SCENE = "Credits";

    private void Start()
    {
        if (pasueMenu != null) 
        {
            creditsButton.SetActive(false);
        }
    }

    public void Back()
    {
        if (mainMenu != null)
        {
            mainMenu.SetActive(true);
            gameObject.SetActive(false);
        }
        else if (pasueMenu != null) 
        {
            pasueMenu.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void Credits() 
    {
        SceneManager.LoadScene(CREDITS_SCENE);
    }

    public void StreamerOverlay() 
    {
        //TODO
        //Enable streamer overlay
        //Likely need to make a static Global settings class
        //Enable in game UI based on global value
    }

    public void Volume() 
    {
        //TODO
        //Need a global volume system to connect to this
    }
}
