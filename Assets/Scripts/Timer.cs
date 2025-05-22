using System;
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
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartTImer();
    }

    // Update is called once per frame
    void Update()
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
    
    private void StartTImer()
    {
        _timeRemaining = _roundDuration;
        _isRunning = true;
    }
    
    private void UpdateTimerVisual(float secondsLeft)
    {
        if (timerText == null) return;

        int mins = Mathf.FloorToInt(secondsLeft / 60f);
        int secs = Mathf.FloorToInt(secondsLeft % 60f);
        timerText.text = $"{mins:00}:{secs:00}";    
    }
    
    public float GetTimeRemaining() => _timeRemaining;
}
