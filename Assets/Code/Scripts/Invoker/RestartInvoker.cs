using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SceneTransitionRunner))]
public class RestartInvoker : MonoBehaviour
{
    private SceneTransitionRunner _runner;

    private void Awake()
    {
        _runner = GetComponent<SceneTransitionRunner>();
    }

    public void Invoke()
    {
        // Restart the game
        // _runner.RestartCurrentScene();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
