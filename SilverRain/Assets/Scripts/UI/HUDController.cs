using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

public class HUDController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI level;

    [SerializeField] private GameObject buffsList;
    [SerializeField] private GameObject killInfo;
    [SerializeField] private GameObject buffPrefab;
    [SerializeField] private GameObject killInfoPrefab;

    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider expBar;

    [SerializeField] private Image damageOverlay;

    [SerializeField] private GameObject GameOverScreen;

    private float timerRemaining = 300f;
    private bool isTimerRunning = true;

    private ConsoleManager consoleManager;
    private GameObject player;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Initialize();

        consoleManager = GameManager.Instance.ConsoleManager;
        if (consoleManager != null)
        {
            RegisterCommands();
        }

        player = GameManager.Instance.Player;

        //Subscribe to events
        PlayerLevel.OnLevelUp += OnLevelUp;
    }

    //Unsubscribe from events
    private void OnDisable() { PlayerLevel.OnLevelUp -= OnLevelUp; }
    private void OnDestroy() { PlayerLevel.OnLevelUp -= OnLevelUp; }

    private void OnLevelUp()
    {
        level.text = ($"Level {player.GetComponent<PlayerLevel>().CurrentPlayerLevel.ToString()}");
    }
    private void RegisterCommands()
    {
        consoleManager.RegisterCommand("timer", args =>
        {
            if (args.Length > 0 && float.TryParse(args[0], out float newTime))
            {
                timerRemaining = newTime;
                consoleManager.AppendOutput($"Reset the timer to {Mathf.FloorToInt(newTime / 60)}:{Mathf.FloorToInt(newTime % 60)}");
            }
            else if (args.Length == 0)
            {
                timerRemaining = 300f;
                consoleManager.AppendOutput("Reset the timer to 5:00");
            }
            else
            {
                consoleManager.AppendOutput("Invalid time amount. (in seconds)");
            }

        }, "<value> - Set the timer in seconds");
        consoleManager.RegisterCommand("pause_timer", args =>
        {
            isTimerRunning = !isTimerRunning;
            consoleManager.AppendOutput(isTimerRunning ? "Resumed the timer" : "Paused the timer");
        }, "- Switch pause the timer");
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimerRunning)
        {
            if (timerRemaining > 0)
            {
                timerRemaining -= Time.deltaTime;
                UpdateTimer(timerRemaining);
            }
            else
            {
                Debug.Log("Time's up!");
                // end game
            }
        }
        healthBar.value = player.GetComponent<PlayerHealth>().GetHealthPercentage();
        expBar.value = player.GetComponent<PlayerLevel>().GetXPPercentage();
        //score.text = FindAnyObjectByType<PlayerStats>().score.ToString();
    }

    void Initialize()
    {
        isTimerRunning = true;
        UpdateTimer(300f);
        score.text = "0";
        level.text = "Level 1";
        healthBar.value = 1;
        expBar.value = 0;
        // delete all children of buffsList and killInfo
        foreach (Transform child in buffsList.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in killInfo.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void UpdateTimer(float time)
    {
        time += 1;

        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void UpdateScore(int newScore)
    {
        score.text = newScore.ToString();
    }

    public void UpdateInventoryIcons()
    {
        // Clear existing icons
        foreach (Transform child in buffsList.transform)
        {
            Destroy(child.gameObject);
        }

        // Get all owned items from PlayerInventory
        var inventory = FindAnyObjectByType<PlayerInventory>();
        if (inventory == null) return;
        foreach (var item in inventory.ownedItems)
        {
            // intantiate corresponding buff prefab
            var newBuff = Instantiate(buffPrefab, buffsList.transform);
            var iconImage = newBuff.GetComponentInChildren<Image>();
            iconImage.sprite = item.icon;
            var tmp = newBuff.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = item.currentLevel.ToString();

        }
    }

    //public void AddBuff(string buffName)
    //{
    //    foreach (GameObject prefab in buffPrefabs)
    //    {
    //        if (prefab.name == buffName)
    //        {
    //            GameObject newBuff = Instantiate(prefab, buffsList.transform);
    //            return;
    //        }
    //    }
    //}

    public void SpownKillInfo(string enemyType)
    {
        string infoText = "";
        switch (enemyType)
        {
            case "1":
                infoText = "Type 1  100";
                break;
            case "2":
                infoText = "Type 2  150";
                break;
            case "3":
                infoText = "Type 3  250";
                break;
            default:
                return;
        }

        if (killInfo.transform.childCount >= 5)
        {
            Destroy(killInfo.transform.GetChild(0).gameObject);
        }

        GameObject newKillInfo = Instantiate(killInfoPrefab, killInfo.transform);
        var tmp = newKillInfo.GetComponent<TextMeshProUGUI>();
        tmp.text = infoText;
        StartCoroutine(FadeAndDestroyKillInfo(tmp, newKillInfo, 1f, 0.5f));
    }

    private IEnumerator FadeAndDestroyKillInfo(TextMeshProUGUI tmp, GameObject obj, float showTime, float fadeTime)
    {
        yield return new WaitForSeconds(showTime);

        Color color = tmp.color;
        float elapsed = 0f;
        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsed / fadeTime);
            tmp.color = color;
            yield return null;
        }
        Destroy(obj);
    }

    public void ShowDamageEffect()
    {
        if (damageOverlay != null)
        {
            StopCoroutine(nameof(FadeDamageOverlay));
            StartCoroutine(FadeDamageOverlay(0.3f, 0.5f));
        }
    }

    private IEnumerator FadeDamageOverlay(float duration, float maxAlpha)
    {
        Color color = damageOverlay.color;
        color.a = maxAlpha;
        damageOverlay.color = color;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(maxAlpha, 0f, elapsed / duration);
            damageOverlay.color = color;
            yield return null;
        }
        color.a = 0f;
        damageOverlay.color = color;
    }

    public void ShowGameOverScreen(bool isWin)
    {
        if (GameOverScreen != null)
        {
            GameOverScreen.SetActive(true);
        }

        if (isWin)
        {
            // Show win message
        }
        else
        {
            // Show lose message
        }
    }
}
