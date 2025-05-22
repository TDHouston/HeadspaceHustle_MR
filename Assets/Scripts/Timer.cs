using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] private float _roundDuration = 60f;
    [SerializeField] private TextMeshProUGUI timerText;
    public UnityEvent OnTimeUp;

    private float _timeRemaining;
    private bool _isRunning = false;

    public void StartTimer()
    {
        _timeRemaining = _roundDuration;
        _isRunning = true;
    }

    public void StopTimer()
    {
        _isRunning = false;
    }

    public float GetTimeRemaining() => _timeRemaining;

    private void Update()
    {
        if (!_isRunning) return;

        _timeRemaining -= Time.deltaTime;
        UpdateTimerVisual(_timeRemaining);

        if (_timeRemaining <= 0)
        {
            _isRunning = false;
            _timeRemaining = 0;
            OnTimeUp.Invoke();
        }
    }

    private void UpdateTimerVisual(float secondsLeft)
    {
        if (timerText == null) return;

        int mins = Mathf.FloorToInt(secondsLeft / 60f);
        int secs = Mathf.FloorToInt(secondsLeft % 60f);
        timerText.text = $"{mins:00}:{secs:00}";
    }
}