using UnityEngine;

public class CarClick : MonoBehaviour
{
    public GameManager gameManager;

    void OnMouseDown()
    {
        if (gameManager != null)
        {
            gameManager.OnCarClicked(transform);
        }
    }
}