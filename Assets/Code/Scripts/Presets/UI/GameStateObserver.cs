using DhafinFawwaz.AnimationUI;
using UnityEngine;

public class GameStateObserver : MonoBehaviour
{
    [SerializeField] AnimationUI _winShowAnimation;
    [SerializeField] AnimationUI _loseShowAnimation;

    void OnEnable() {
        GameManager.S_OnGameStateChanged += HandleGameStateChanged;
    }

    void OnDisable() {
        GameManager.S_OnGameStateChanged -= HandleGameStateChanged;
    }

    void HandleGameStateChanged(GameState state) {
        if (state == GameState.Win) {
            _winShowAnimation.Stop();
            _winShowAnimation.SetAllTweenTargetValueAsFrom();
            _winShowAnimation.Play();
        } else if (state == GameState.Lose) {
            _loseShowAnimation.Stop();
            _loseShowAnimation.SetAllTweenTargetValueAsFrom();
            _loseShowAnimation.Play();
        }
    }
}
