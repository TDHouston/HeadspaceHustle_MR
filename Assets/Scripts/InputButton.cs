using UnityEngine;
using UnityEngine.UI;

public class InputButton : MonoBehaviour
{
    public string value;
    public MemoryTask memoryTask;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => memoryTask.SubmitInput(value));
    }
}

