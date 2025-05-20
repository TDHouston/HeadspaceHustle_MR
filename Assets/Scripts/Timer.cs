using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private float _timer = 60f;
    [SerializeField] private bool _isRunning = false;
    public event Action OnTimerEnd;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartTImer();
    }

    private void StartTImer()
    {
        _timer = 60f;
        _isRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isRunning) return;
        
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            _isRunning = false;
            OnTimerEnd?.Invoke();
        }
    }
    
    public float GetTimeRemaining()
    {
        return _timer;
    }
}
