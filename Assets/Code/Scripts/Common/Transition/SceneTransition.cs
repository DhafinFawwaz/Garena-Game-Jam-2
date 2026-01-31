using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneTransition : MonoBehaviour
{
    [Tooltip("Delay before starting the transition in seconds")]
    [SerializeField] float _delayBeforeInTransition = 0.2f;
    public void LoadScene(string sceneName) {
        var transition = Instantiate(this);
        DontDestroyOnLoad(transition.gameObject);
        transition.StartTransition(sceneName);
    }

    void StartTransition(string sceneName) {
        StartCoroutine(AsynchronousTransition(sceneName));
    }
    IEnumerator AsynchronousTransition(string sceneName) {
        yield return TransitionOutAnimation();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone) {
            if (asyncLoad.progress >= 0.9f) {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
        yield return new WaitForSeconds(_delayBeforeInTransition);
        yield return TransitionInAnimation();
        Destroy(gameObject);
    }

    protected virtual IEnumerator TransitionOutAnimation() {
        yield return new WaitForSeconds(0.5f);
    }

    protected virtual IEnumerator TransitionInAnimation() {
        yield return new WaitForSeconds(0.5f);
    }
}
