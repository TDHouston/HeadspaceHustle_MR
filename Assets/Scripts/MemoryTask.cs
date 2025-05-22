using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MemoryTask : Taskbase
{
    [Header("UI")] 
    public TextMeshProUGUI displayText;
    public Button startButton;
    public GameObject inputButtonsParent;

    [Header("Sequence Settings")] 
    public string[] sequenceOptions = { "A", "B", "C", "D" };
    public float showDelay = 1f;
    public Color[] sequenceColors;

    [Header("References")] 
    public TaskManager taskManager;

    private List<string> sequence = new();
    private int inputIndex = 0;
    private bool isInputActive = false;
    private int round = 1;
    private bool isRunning = false;
    private bool inputCooldown = false;

    private void Awake()
    {
        ShowStartState();
    }

    private void Start()
    {
        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(BeginTask);
    }

    public override void BeginTask()
    {
        if (isRunning) return;

        isRunning = true;
        isInputActive = false;

        displayText.text = "";
        displayText.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        inputButtonsParent.SetActive(false);

        taskManager?.NotifyTaskStarted(this);

        sequence.Clear();
        GenerateSequence(round);
        StartCoroutine(ShowSequence());
    }

    public override void EndTask()
    {
        StopAllCoroutines();
        isRunning = false;
        isInputActive = false;
        inputButtonsParent.SetActive(false);
        ShowStartState();
    }

    private void GenerateSequence(int length)
    {
        for (int i = 0; i < length + 2; i++)
        {
            sequence.Add(sequenceOptions[Random.Range(0, sequenceOptions.Length)]);
        }
    }

    private IEnumerator ShowSequence()
    {
        displayText.text = "";
        displayText.gameObject.SetActive(false);
        inputButtonsParent.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < sequence.Count; i++)
        {
            displayText.gameObject.SetActive(true);
            displayText.text = sequence[i];

            if (sequenceColors.Length > 0)
                displayText.color = sequenceColors[i % sequenceColors.Length];

            yield return new WaitForSeconds(showDelay);
            displayText.text = "";
            displayText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.25f);
        }

        displayText.color = Color.white;
        displayText.text = "Repeat!";
        displayText.gameObject.SetActive(true);

        inputIndex = 0;
        isInputActive = true;
        inputButtonsParent.SetActive(true);

        Debug.Log($"Ready for input. Sequence: {string.Join(",", sequence)}");
    }

    public void SubmitInput(string input)
    {
        if (!isInputActive || inputCooldown) return;

        Debug.Log($"Submitted input: {input}, Expected: {sequence[inputIndex]} (Index: {inputIndex})");

        StartCoroutine(InputCooldownDelay());

        if (input == sequence[inputIndex])
        {
            inputIndex++;

            if (inputIndex >= sequence.Count)
            {
                displayText.text = $"Round {round} complete!";
                round++;
                isInputActive = false;
                inputButtonsParent.SetActive(false);
                StartCoroutine(NextRound());
            }
        }
        else
        {
            Debug.LogWarning($"Input mismatch: got '{input}', expected '{sequence[inputIndex]}'");

            displayText.text = "Wrong! Try again.";
            displayText.gameObject.SetActive(true);
            isInputActive = false;
            inputButtonsParent.SetActive(false);
            isRunning = false;

            // Small delay before re-enabling Start button
            StartCoroutine(ShowStartAfterDelay(1.5f));
        }
    }

    private IEnumerator InputCooldownDelay()
    {
        inputCooldown = true;
        yield return new WaitForSeconds(0.25f);
        inputCooldown = false;
    }

    private IEnumerator NextRound()
    {
        yield return new WaitForSeconds(2f);
        displayText.gameObject.SetActive(false);
        BeginTask();
    }

    private IEnumerator ShowStartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowStartState();
    }

    private void ShowStartState()
    {
        displayText.gameObject.SetActive(false);
        startButton.gameObject.SetActive(true);
        inputButtonsParent.SetActive(false);
    }
}