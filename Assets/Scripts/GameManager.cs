using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Game References")] 
    public Timer timer;
    public MemoryTask memoryTask;
    public RoundHandler roundHandler;

    [Header("UI Elements")] 
    public GameObject timerPanel;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI messageText;
    public Button retryButton;

    private bool gameEnded = false;

    void Start()
    {
        retryButton.onClick.AddListener(RestartGame);
        retryButton.gameObject.SetActive(false);
        messageText.text = "";

        timer.OnTimeUp.AddListener(HandleTimeUp);
    }

    void HandleTimeUp()
    {
        if (gameEnded) return;

        gameEnded = true;
        memoryTask.EndTask();

        timerText.gameObject.SetActive(false);
        messageText.text = "⏰ Time's up! Try again?";
        retryButton.gameObject.SetActive(true);
    }

    public void HandleTaskCompleted()
    {
        if (gameEnded) return;

        gameEnded = true;
        timer.enabled = false;

        timerText.gameObject.SetActive(false);
        messageText.text = "🎉 You did it! Play again?";
        retryButton.gameObject.SetActive(true);
    }

    void RestartGame()
    {
        roundHandler.RestartScene();
    }
}