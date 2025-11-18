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
    [SerializeField] private GameObject buffPrefab;
    [SerializeField] private GameObject killInfoPrefab;

    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider expBar;

    [SerializeField] private Image damageOverlay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        timer.text = Time.time.ToString("00:00");
        healthBar.value = FindAnyObjectByType<PlayerHealth>().GetHealthPercentage();
        expBar.value = FindAnyObjectByType<PlayerLevel>().GetXPPercentage();
        score.text = FindAnyObjectByType<PlayerStats>().score.ToString();
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
}
