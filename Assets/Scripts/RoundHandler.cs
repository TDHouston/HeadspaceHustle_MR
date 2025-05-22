using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundHandler : MonoBehaviour
{
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
