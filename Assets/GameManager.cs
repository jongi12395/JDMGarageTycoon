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

    [Header("Click Upgrade")]
    public int clickLevel = 1;
    public int clickCost = 30;
    public int clickIncrease = 5;

    [Header("UI")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI engineText;
    public TextMeshProUGUI clickText;
    public Button engineButton;
    public Button clickButton;

    [Header("Car")]
    public Transform car;

    [Header("Floating Text")]
    public GameObject floatingTextPrefab;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clickSound;
    public AudioClip upgradeSound;

    void Start()
    {
        UpdateUI();
        StartCoroutine(AddMoneyOverTime());
    }

    IEnumerator AddMoneyOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            money += moneyPerSecond;
            UpdateUI();
        }
    }

    public void OnCarClicked()
    {
        money += clickValue;

        PlaySound(clickSound);
        SpawnFloatingText();
        StartCoroutine(AnimateCar());

        UpdateUI();
    }

    public void BuyEngine()
    {
        if (money >= engineCost)
        {
            money -= engineCost;
            engineLevel++;
            moneyPerSecond += engineIncrease;
            engineCost += 25;

            PlaySound(upgradeSound);
            StartCoroutine(AnimateCar());
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
            clickCost += 15;

            PlaySound(upgradeSound);
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        moneyText.text = "Money: $" + money + "\nIncome: $" + moneyPerSecond + "/sec";

        engineText.text = "Engine Lv. " + engineLevel + "\nCost: $" + engineCost;
        clickText.text = "Click Lv. " + clickLevel + "\nCost: $" + clickCost;

        engineButton.interactable = money >= engineCost;
        clickButton.interactable = money >= clickCost;
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    void SpawnFloatingText()
    {
        GameObject text = Instantiate(floatingTextPrefab, moneyText.transform.parent);
        text.SetActive(true);

        TextMeshProUGUI tmp = text.GetComponent<TextMeshProUGUI>();
        tmp.text = "+$" + clickValue;

        RectTransform rt = text.GetComponent<RectTransform>();
        Vector3 screenPos = Camera.main.WorldToScreenPoint(car.position);
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

    IEnumerator AnimateCar()
    {
        if (car == null) yield break;

        Vector3 originalScale = car.localScale;
        Vector3 popScale = originalScale * 1.15f;

        float duration = 0.08f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            car.localScale = Vector3.Lerp(originalScale, popScale, time / duration);
            yield return null;
        }

        time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            car.localScale = Vector3.Lerp(popScale, originalScale, time / duration);
            yield return null;
        }

        car.localScale = originalScale;
    }
}