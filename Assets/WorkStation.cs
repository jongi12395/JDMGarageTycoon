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
    public Color highlightColor = Color.yellow;

    private Vector3 originalScale;
    private Renderer stationRenderer;
    private Color originalColor;

    void Start()
    {
        originalScale = transform.localScale;

        stationRenderer = GetComponent<Renderer>();

        if (stationRenderer != null)
        {
            originalColor = stationRenderer.material.color;
        }
    }

    public void SetWorkingVisual(bool isWorking)
    {
        transform.localScale = isWorking ? originalScale * pulseScale : originalScale;
    }

    public void SetMissionHighlight(bool isHighlighted)
    {
        if (stationRenderer == null) return;

        stationRenderer.material.color = isHighlighted ? highlightColor : originalColor;
    }

    public void DoWork(GameManager gameManager)
    {
        if (gameManager != null)
        {
            gameManager.TryCompleteStation(this);
        }
    }
}