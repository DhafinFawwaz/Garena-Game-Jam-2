using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] SceneTransition _transitionPrefab;
    [SerializeField] string _firstSceneName = "2";
    public void ToGameScene() {
        _transitionPrefab.LoadScene(_firstSceneName);
    }

    public void Exit() {
        Application.Quit();
    }


}
