using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private Taskbase[] tasks;
    [SerializeField] private float taskDuration = 10f;

    private Taskbase _currentTask;
    private int _currentTaskIndex = -1;
    private float _taskTimer = 0f;
    private bool _taskActive = false;

    void Start()
    {
        _currentTaskIndex = 0;
        _currentTask = tasks[_currentTaskIndex];
        _taskActive = false;
    }

    void Update()
    {
        if (!_taskActive || _currentTask == null) return;

        _taskTimer -= Time.deltaTime;

        if (_taskTimer <= 0f)
        {
            EndCurrentTask();
            StartNextTask();
        }
    }

    public void NotifyTaskStarted(Taskbase task)
    {
        if (_currentTask != task) return;

        Debug.Log($"TaskManager: Detected start of task {task.name}");
        _taskActive = true;
        _taskTimer = taskDuration;
    }

    void EndCurrentTask()
    {
        if (_currentTask != null)
        {
            _currentTask.EndTask();
        }

        _taskActive = false;
        _currentTask = null;
    }

    void StartNextTask()
    {
        _currentTaskIndex = (_currentTaskIndex + 1) % tasks.Length;
        _currentTask = tasks[_currentTaskIndex];
        _taskActive = false;

        // Do not auto-start the task — it must be manually triggered
        Debug.Log($"TaskManager: Ready for next task { _currentTask.name } to be started manually.");
    }
}