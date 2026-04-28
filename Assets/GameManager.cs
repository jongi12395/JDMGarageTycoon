using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float money = 0f;
    public float moneyPerSecond = 5f;
    public float clickValue = 10f;

    public TextMeshProUGUI moneyText;

    void Update()
    {
        // Passive income
        money += moneyPerSecond * Time.deltaTime;

        // Update UI
        moneyText.text = "Money: $" + Mathf.FloorToInt(money);

        // Click detection
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name == "Cube")
                {
                    money += clickValue;
                }
            }
        }
    }
}