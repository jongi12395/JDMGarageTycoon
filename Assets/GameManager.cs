using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float money = 0f;
    public float moneyPerSecond = 5f;
    public float clickValue = 10f;

    public float upgradeCost = 50f;
    public float upgradeIncrease = 5f;

    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI upgradeText;
    public Button upgradeButton;

    void Update()
    {
        money += moneyPerSecond * Time.deltaTime;

        moneyText.text = "Money: $" + Mathf.FloorToInt(money);
        upgradeText.text = "Upgrade Engine ($" + Mathf.FloorToInt(upgradeCost) + ")";

        upgradeButton.interactable = money >= upgradeCost;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.name == "Cube")
                {
                    money += clickValue;
                }
            }
        }
    }

    public void BuyEngine()
    {
        if (money >= upgradeCost)
        {
            money -= upgradeCost;
            moneyPerSecond += upgradeIncrease;

            upgradeCost *= 1.5f;
            upgradeCost = Mathf.Round(upgradeCost);
        }
    }
}