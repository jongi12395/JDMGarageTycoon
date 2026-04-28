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

    [Header("Upgrade System")]
    public int upgradeCost = 50;
    public int upgradeIncrease = 5;

    [Header("UI")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI upgradeText;
    public Button upgradeButton;

    [Header("Floating Text")]
    public GameObject floatingTextPrefab;

    [Header("Car")]
    public Transform car;

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
        StartCoroutine(AnimateCar());
        SpawnFloatingText();
        UpdateUI();
    }

    public void BuyEngine()
    {
        if (money >= upgradeCost)
        {
            money -= upgradeCost;
            moneyPerSecond += upgradeIncrease;
            upgradeCost = Mathf.RoundToInt(upgradeCost * 1.5f);

            StartCoroutine(AnimateCar());
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        moneyText.text = "Money: $" + money;
        upgradeText.text = "Upgrade Engine\n($" + upgradeCost + ")";

        upgradeButton.interactable = money >= upgradeCost;
    }

    void SpawnFloatingText()
    {
        GameObject text = Instantiate(floatingTextPrefab, moneyText.transform.parent);
        text.SetActive(true);

        TextMeshProUGUI tmp = text.GetComponent<TextMeshProUGUI>();
        tmp.text = "+$" + clickValue;

        RectTransform rt = text.GetComponent<RectTransform>();

        // Convert world position → screen position
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

            // Move upward
            rt.anchoredPosition = startPos + Vector3.up * (time * 80f);

            // Fade out
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
        car.localScale = originalScale * 1.2f;

        yield return new WaitForSeconds(0.1f);

        car.localScale = originalScale;
    }
}