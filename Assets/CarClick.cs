using UnityEngine;

public class CarClick : MonoBehaviour
{
    public GameManager gameManager;

    void OnMouseDown()
    {
        gameManager.OnCarClicked();
    }
}