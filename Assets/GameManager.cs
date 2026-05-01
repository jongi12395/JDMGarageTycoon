using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Money System")]
    public int money = 0;
    public int moneyPerSecond = 5;
    public int clickValue = 10;

    [Header("Engine Upgrade")]
    public int engineLevel = 1;
    public int engineCost = 50;
    public int engineIncrease = 5;
    public float engineCostMultiplier = 1.35f;

    [Header("Click Upgrade")]
    public int clickLevel = 1;
    public int clickCost = 30;
    public int clickIncrease = 5;
    public float clickCostMultiplier = 1.35f;

    [Header("Mission System")]
    public int missionTarget = 3;
    public int missionProgress = 0;
    public int missionReward = 100;
    public TextMeshProUGUI missionText;

    [Header("UI")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI engineText;
    public TextMeshProUGUI clickText;
    public Button engineButton;
    public Button clickButton;

    [Header("Floating Text")]
    public GameObject floatingTextPrefab;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clickSound;
    public AudioClip upgradeSound;

    private Coroutine carAnimation;

    void Start()
    {
        UpdateUI();
        UpdateMissionUI();
        StartCoroutine(AddMoneyOverTime());
    }

    IEnumerator AddMoneyOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            AddMoney(moneyPerSecond, false, Vector3.zero);
        }
    }

    public void OnCarClicked(Transform carTransform)
    {
        AddMoney(clickValue, true, carTransform.position);
        PlayCarAnimation(carTransform);
    }

    public void AddMoney(int amount, bool showFeedback, Vector3 worldPosition)
    {
        money += amount;

        if (showFeedback)
        {
            PlaySound(clickSound);
            SpawnFloatingText(amount, worldPosition);
        }

        UpdateUI();
    }

    public void CompleteWork(Vector3 stationPosition, int rewardAmount)
    {
        AddMoney(rewardAmount, true, stationPosition);

        missionProgress++;

        if (missionProgress >= missionTarget)
        {
            AddMoney(missionReward, true, stationPosition);
            missionProgress = 0;
        }

        UpdateMissionUI();
    }

    void UpdateMissionUI()
    {
        if (missionText != null)
            missionText.text = "Mission: Fix Starter Car " + missionProgress + "/" + missionTarget;
    }

    public void BuyEngine()
    {
        if (money >= engineCost)
        {
            money -= engineCost;
            engineLevel++;
            moneyPerSecond += engineIncrease;
            engineCost = Mathf.RoundToInt(engineCost * engineCostMultiplier);

            PlaySound(upgradeSound);
            UpdateUI();
        }
    }

    public void BuyClick()
    {
        if (money >= clickCost)
        {
            money -= clickCost;
            clickLevel++;
            clickValue += clickIncrease;
            clickCost = Mathf.RoundToInt(clickCost * clickCostMultiplier);

            PlaySound(upgradeSound);
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        if (moneyText != null)
            moneyText.text = "Money: $" + money + "\nIncome: $" + moneyPerSecond + "/sec";

        if (engineText != null)
            engineText.text = "Engine Lv. " + engineLevel + "\nCost: $" + engineCost;

        if (clickText != null)
            clickText.text = "Click Lv. " + clickLevel + "\nCost: $" + clickCost;

        if (engineButton != null)
            engineButton.interactable = money >= engineCost;

        if (clickButton != null)
            clickButton.interactable = money >= clickCost;
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }

    void PlayCarAnimation(Transform carTransform)
    {
        if (carTransform == null) return;

        if (carAnimation != null)
            StopCoroutine(carAnimation);

        carAnimation = StartCoroutine(AnimateCar(carTransform));
    }

    IEnumerator AnimateCar(Transform carTransform)
    {
        Vector3 originalScale = carTransform.localScale;
        Vector3 smallerScale = originalScale * 0.9f;
        Vector3 popScale = originalScale * 1.15f;

        float duration = 0.05f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            carTransform.localScale = Vector3.Lerp(originalScale, smallerScale, time / duration);
            yield return null;
        }

        time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            carTransform.localScale = Vector3.Lerp(smallerScale, popScale, time / duration);
            yield return null;
        }

        time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            carTransform.localScale = Vector3.Lerp(popScale, originalScale, time / duration);
            yield return null;
        }

        carTransform.localScale = originalScale;
    }

    public void SpawnFloatingText(int amount, Vector3 worldPosition)
    {
        if (floatingTextPrefab == null || moneyText == null) return;

        GameObject text = Instantiate(floatingTextPrefab, moneyText.transform.parent);
        text.SetActive(true);

        TextMeshProUGUI tmp = text.GetComponent<TextMeshProUGUI>();
        tmp.text = "+$" + amount;

        RectTransform rt = text.GetComponent<RectTransform>();
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition + Vector3.up * 1.5f);
        rt.position = screenPos;

        StartCoroutine(AnimateFloatingText(text));
    }

    IEnumerator AnimateFloatingText(GameObject text)
    {
        RectTransform rt = text.GetComponent<RectTransform>();
        TextMeshProUGUI tmp = text.GetComponent<TextMeshProUGUI>();

        Vector3 startPos = rt.anchoredPosition;
        Color startColor = tmp.color;

        float duration = 1f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            rt.anchoredPosition = startPos + Vector3.up * (time * 80f);

            float alpha = Mathf.Lerp(1f, 0f, time / duration);
            tmp.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            yield return null;
        }

        Destroy(text);
    }
}