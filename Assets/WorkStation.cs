using UnityEngine;

public class WorkStation : MonoBehaviour
{
    public enum StationType
    {
        Engine,
        Paint,
        Tuning
    }

    [Header("Station Settings")]
    public StationType stationType;
    public string stationName = "Work Station";
    public int rewardAmount = 25;
    public float workDuration = 1.5f;

    [Header("Visual Feedback")]
    public float pulseScale = 1.08f;

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    // 🔥 THIS is what your error is missing
    public void SetWorkingVisual(bool isWorking)
    {
        if (isWorking)
            transform.localScale = originalScale * pulseScale;
        else
            transform.localScale = originalScale;
    }

    public void DoWork(GameManager gameManager)
    {
        if (gameManager != null)
        {
            gameManager.CompleteWork(transform.position, rewardAmount);
        }
    }
}