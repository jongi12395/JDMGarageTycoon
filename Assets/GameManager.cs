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

    // 🔥 CLICK THE CAR
    public void OnCarClicked()
    {
        money += clickValue;
        StartCoroutine(AnimateCar());
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

        upgradeButton.interactable = (money >= upgradeCost);
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