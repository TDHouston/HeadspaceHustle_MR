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
    public Button[] inputButtons; // Buttons A-D assigned in Inspector

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

    private void Awake()
    {
        displayText.text = "";
        inputButtonsParent.SetActive(false);
        startButton.gameObject.SetActive(true);
    }

    private void Start()
    {
        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(() =>
        {
            Debug.Log("Start button pressed.");
            BeginTask();
        });

        AssignInputButtons();
    }

    private void AssignInputButtons()
    {
        string[] values = { "A", "B", "C", "D" };

        for (int i = 0; i < inputButtons.Length && i < values.Length; i++)
        {
            string value = values[i];
            inputButtons[i].onClick.RemoveAllListeners();
            inputButtons[i].onClick.AddListener(() => SubmitInput(value));
        }
    }

    public override void BeginTask()
    {
        if (isRunning) return;

        isRunning = true;
        isInputActive = false;

        displayText.text = "";
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
        displayText.text = "Press Start";
        startButton.gameObject.SetActive(true);
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
        inputButtonsParent.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < sequence.Count; i++)
        {
            displayText.text = sequence[i];

            if (sequenceColors.Length > 0)
                displayText.color = sequenceColors[i % sequenceColors.Length];

            yield return new WaitForSeconds(showDelay);
            displayText.text = "";
            yield return new WaitForSeconds(0.25f);
        }

        displayText.color = Color.white;
        displayText.text = "Repeat!";
        inputIndex = 0;
        isInputActive = true;
        inputButtonsParent.SetActive(true);
    }

    public void SubmitInput(string input)
    {
        if (!isInputActive) return;

        if (input == sequence[inputIndex])
        {
            inputIndex++;

            if (inputIndex >= sequence.Count)
            {
                displayText.text = $"✅ Round {round} complete!";
                round++;
                isInputActive = false;
                inputButtonsParent.SetActive(false);
                StartCoroutine(NextRound());
            }
        }
        else
        {
            displayText.text = "❌ Wrong! Try again.";
            isInputActive = false;
            inputButtonsParent.SetActive(false);
            isRunning = false;
            startButton.gameObject.SetActive(true);
        }
    }

    private IEnumerator NextRound()
    {
        yield return new WaitForSeconds(2f);
        BeginTask();
    }
}
