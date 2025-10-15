using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    HUDController instance;
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

    public void SpownKillInfo(string killType)
    {
        switch (killType)
        {
            case "killType1":
                {
                    GameObject newKillInfo = Instantiate(killInfoPrefab, killInfo.transform);
                    newKillInfo.GetComponent<TextMeshPro>().text = "killType 1";
                    break;
                }
            case "Melee":
                {
                    GameObject newKillInfo = Instantiate(killInfoPrefab, killInfo.transform);
                    newKillInfo.GetComponent<TextMeshPro>().text = "Melee Kill!";
                    break;
                }
            case "Explosion":
                {
                    GameObject newKillInfo = Instantiate(killInfoPrefab, killInfo.transform);
                    newKillInfo.GetComponent<TextMeshPro>().text = "Explosion Kill!";
                    break;
                }
            default:
                {
                    GameObject newKillInfo = Instantiate(killInfoPrefab, killInfo.transform);
                    newKillInfo.GetComponent<TextMeshPro>().text = "Kill!";
                    break;
                }
        }
    }
}
