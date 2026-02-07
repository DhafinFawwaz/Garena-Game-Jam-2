using UnityEngine;

public class SceneTransitionRunner : MonoBehaviour
{
    [SerializeField] SceneTransition _transitionPrefab;
    [SerializeField] string _sceneName;

    public void StartSceneTransition() {
        SceneTransition transition = Instantiate(_transitionPrefab);
        transition.LoadScene(_sceneName);
    }

    public void StartSceneTransition(string sceneName) {
        SceneTransition transition = Instantiate(_transitionPrefab);
        transition.LoadScene(sceneName);
    }

    /// <summary>
    /// Helper method to transition to the next scene in the build index. Useful for development and testing purposes.
    /// </summary>
    public void StartSceneTransitionToNextBuildIndex() {
        int nextBuildIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1;
        if (nextBuildIndex >= UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings) {
            nextBuildIndex = 0;
        }
        _transitionPrefab.LoadScene(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(nextBuildIndex));
    }

    public void StartSceneTransitionToPreviousBuildIndex() {
        int previousBuildIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex - 1;
        if (previousBuildIndex < 0) {
            previousBuildIndex = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings - 1;
        }
        _transitionPrefab.LoadScene(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(previousBuildIndex));
    }

    public void RestartCurrentScene() {
        string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        SceneTransition transition = Instantiate(_transitionPrefab);
        transition.LoadScene(currentSceneName);
    }
}
