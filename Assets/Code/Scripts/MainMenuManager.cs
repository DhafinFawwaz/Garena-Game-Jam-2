using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] SceneTransition _transitionPrefab;
    string _firstSceneName = "Game";
    public void ToGameScene() {
        // _transitionPrefab.LoadScene(_firstSceneName);
        SceneManager.LoadScene(_firstSceneName);
    }

    void Update() {
        if(Keyboard.current.anyKey.wasPressedThisFrame) {
            ToGameScene();
        }
    }

    public void Exit() {
        Application.Quit();
    }


}
