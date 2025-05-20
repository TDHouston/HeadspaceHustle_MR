using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public GameObject[] taskZones; // Drag Memory, Impulse, Emotion here
    private int currentTaskIndex = -1;

    public float taskDuration = 10f;
    private float taskTimer;
    private bool taskActive = false;

    void Start()
    {
        Timer timer = GetComponent<Timer>();
        if (timer != null)
        {
            timer.OnTimerEnd += HandleTimerEnd;
        }
        StartNextTask();
    }

    void Update()
    {
        if (!taskActive) return;

        taskTimer -= Time.deltaTime;
        if (taskTimer <= 0)
        {
            EndCurrentTask();
            StartNextTask();
        }
    }

    void StartNextTask()
    {
        currentTaskIndex = (currentTaskIndex + 1) % taskZones.Length;
        GameObject activeZone = taskZones[currentTaskIndex];
        // Activate visuals / start interaction
        Debug.Log("Starting task: " + activeZone.name);
        taskTimer = taskDuration;
        taskActive = true;
    }

    void EndCurrentTask()
    {
        GameObject endingZone = taskZones[currentTaskIndex];
        Debug.Log("Ending task: " + endingZone.name);
        // Deactivate visuals / reset state
        taskActive = false;
    }
    
    void HandleTimerEnd()
    {
        Debug.Log("Game Over! No more tasks.");
        taskActive = false;
    }
}