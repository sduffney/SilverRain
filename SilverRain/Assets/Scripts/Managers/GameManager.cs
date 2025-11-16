using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    // In future, it may be possible to integrate some of the other Manager's functions
    // into the GameManager.

    private PlayerInput playerInput;



    // pause manager
    private int pauseCounter = 0;


    // Singleton instance
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
    }


    #region Pause Management
    public void RequestPause()
    {
        pauseCounter++;
        UpdatePauseState();
    }

    public void ReleasePause()
    {
        pauseCounter = Mathf.Max(0, pauseCounter - 1);
        UpdatePauseState();
    }

    private void UpdatePauseState()
    {
        if (pauseCounter > 0)
        {
            Time.timeScale = 0f;
            playerInput.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1f;
            playerInput.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
#endregion
}
