using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionRange = 3f;

    [Header("References")]
    public GameManager gameManager;
    public TextMeshProUGUI interactionPrompt;
    public Slider workProgressBar;

    private WorkStation currentStation;
    private WorkStation previousStation;
    private float workTimer = 0f;

    void Start()
    {
        if (interactionPrompt != null)
            interactionPrompt.gameObject.SetActive(false);

        if (workProgressBar != null)
        {
            workProgressBar.gameObject.SetActive(false);
            workProgressBar.value = 0f;
        }
    }

    void Update()
    {
        FindClosestStation();
        UpdatePrompt();

        if (currentStation != null && Input.GetKey(KeyCode.E))
        {
            WorkOnStation();
        }
        else
        {
            ResetWork();
        }
    }

    void FindClosestStation()
    {
        previousStation = currentStation;
        currentStation = null;

        WorkStation[] stations = FindObjectsOfType<WorkStation>();
        float closestDistance = interactionRange;

        foreach (WorkStation station in stations)
        {
            float distance = Vector3.Distance(transform.position, station.transform.position);

            if (distance <= closestDistance)
            {
                closestDistance = distance;
                currentStation = station;
            }
        }

        if (previousStation != null && previousStation != currentStation)
        {
            previousStation.SetWorkingVisual(false);
        }
    }

    void UpdatePrompt()
    {
        if (interactionPrompt == null)
            return;

        bool hasStation = currentStation != null;
        interactionPrompt.gameObject.SetActive(hasStation);

        if (hasStation)
        {
            interactionPrompt.text = "Hold E to work on " + currentStation.stationName;
        }
    }

    void WorkOnStation()
    {
        if (currentStation == null || gameManager == null)
            return;

        currentStation.SetWorkingVisual(true);

        workTimer += Time.deltaTime;

        if (workProgressBar != null)
        {
            workProgressBar.gameObject.SetActive(true);
            workProgressBar.value = workTimer / currentStation.workDuration;
        }

        if (workTimer >= currentStation.workDuration)
        {
            currentStation.DoWork(gameManager);

            Debug.Log("Completed: " + currentStation.stationName);

            workTimer = 0f;

            currentStation.SetWorkingVisual(false);

            if (workProgressBar != null)
                workProgressBar.value = 0f;
        }
    }

    void ResetWork()
    {
        workTimer = 0f;

        if (currentStation != null)
            currentStation.SetWorkingVisual(false);

        if (workProgressBar != null)
        {
            workProgressBar.value = 0f;
            workProgressBar.gameObject.SetActive(false);
        }
    }
}