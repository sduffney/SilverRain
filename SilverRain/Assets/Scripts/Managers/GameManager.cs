using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // In future, it may be possible to integrate some of the other Manager's functions
    // into the GameManager.

    [Header("Player")]
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerStats playerStats;

    //Globally accessible getters
    public GameObject Player => player;
    public PlayerController PlayerController => playerController;
    public PlayerStats PlayerStats => playerStats;

    [Header("Managers")]
    [SerializeField] private PermanentUpgradeManager permanentUpgradeManager;
    [SerializeField] private TemporaryUpgradeManager temporaryUpgradeManager;
    [SerializeField] private ConsoleManager consoleManager;
    [SerializeField] private BuffManager buffManager;
    [SerializeField] private HUDController hudController;

    //Globally accessible getters
    public PermanentUpgradeManager PermanentUpgradeManager => permanentUpgradeManager;
    public TemporaryUpgradeManager TemporaryUpgradeManager => temporaryUpgradeManager;
    public ConsoleManager ConsoleManager => consoleManager;
    public BuffManager BuffManager => buffManager;
    public HUDController HUDController => hudController;

    [SerializeField] private float goldMultiplier = 1.0f;

    // Score and Money
    public int Score { get; private set; } = 0;

    // pause manager
    private int pauseCounter = 0;


    // Singleton instance
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        //Singelton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Destroy(gameObject);
        }

        //Get attached components
        permanentUpgradeManager = GetComponent<PermanentUpgradeManager>();
        temporaryUpgradeManager = GetComponent<TemporaryUpgradeManager>();
        consoleManager = GetComponent<ConsoleManager>();
        buffManager = GetComponent<BuffManager>();

        //Reset score
        Score = 0;

        //Subscribe to events
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //Unsubscribe from events
    public void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //Get scene specific references
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Try to get the player in the scene
        player = GameObject.FindWithTag("Player");
        //Get the other components if this scene has a player
        if (player != null)
        {
            playerInput = player.GetComponent<PlayerInput>();
            playerController = player.GetComponent<PlayerController>();
            playerStats = player.GetComponent<PlayerStats>();
            hudController = FindAnyObjectByType<HUDController>();
            if (hudController != null) { buffManager.cardParent = hudController.CardParent; }
        }
    }

    #region GameOver Management

    public void GameOver(bool isWin)
    {
        if (isWin)
        {
            permanentUpgradeManager.AddGold(Mathf.RoundToInt(Score * goldMultiplier));
            hudController.ShowGameOverScreen(isWin);
        }
        else
        {
            permanentUpgradeManager.AddGold(Mathf.RoundToInt(Score * goldMultiplier * 0.5f));
            hudController.ShowGameOverScreen(isWin);
        }
        RequestPause();
    }

    #endregion

    #region Score Management

    public void AddScore(int amount)
    {
        Score += amount;
        hudController.UpdateScore(Score);
    }

    #endregion

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
