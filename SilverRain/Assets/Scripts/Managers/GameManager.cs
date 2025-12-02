using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // In future, it may be possible to integrate some of the other Manager's functions
    // into the GameManager.

    private PlayerInput playerInput;
    private HUDController hudController;
    public GoldManager goldManager;

    [SerializeField] private float goldMultiplier = 1.0f;

    // Score and Money
    public int Score { get; private set; } = 0;

    // pause manager
    private int pauseCounter = 0;


    // Singleton instance
    public static GameManager Instance { get; private set; }
    public GameObject player;

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
        GetReferences();
        goldManager = GetComponent<GoldManager>();

        Score = 0;
    }

    private void GetReferences()
    {
        if (SceneManager.GetActiveScene().name != "Level1")
            return;
        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
        hudController = FindAnyObjectByType<HUDController>();
    }

    #region GameOver Management

    public void GameOver(bool isWin)
    {
        if (isWin)
        {
            goldManager.AddGold(Mathf.RoundToInt(Score * goldMultiplier));
            hudController.ShowGameOverScreen(isWin);
        }
        else
        {
            goldManager.AddGold(Mathf.RoundToInt(Score * goldMultiplier * 0.5f));
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
