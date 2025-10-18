using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class HUDController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI level;

    [SerializeField] private GameObject buffsList;
    [SerializeField] private GameObject killInfo;
    [SerializeField] private GameObject[] buffPrefabs;
    [SerializeField] private GameObject killInfoPrefab;

    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider expBar;

    public HUDController instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            Initialize();
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer(Time.time.ToString("00:00"));

        float healthPercent = FindAnyObjectByType<PlayerHealth>().GetHealthPercentage();
        UpdateHealth(healthPercent);

        float expPercent = FindAnyObjectByType<PlayerLevel>().GetXPPercentage();
        UpdateExp(expPercent);

        UpdateScore(FindAnyObjectByType<PlayerStats>().score);
    }

    void Initialize()
    {
        timer.text = "00:00";
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

    public void UpdateTimer(string time)
    {
        timer.text = time;
    }

    public void UpdateScore(int newScore)
    {
        score.text = newScore.ToString();
    }

    public void UpdateLevel(string newLevel)
    {
        level.text = newLevel;
    }

    public void UpdateHealth(float healthPercent)
    {
        healthBar.value = healthPercent;
    }

    public void UpdateExp(float expPercent)
    {
        expBar.value = expPercent;
    }

    public void AddBuff(string buffName)
    {
        foreach (GameObject prefab in buffPrefabs)
        {
            if (prefab.name == buffName)
            {
                GameObject newBuff = Instantiate(prefab, buffsList.transform);
                return;
            }
        }
    }

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
        yield  return new WaitForSeconds(showTime);

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
}
