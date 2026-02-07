using DhafinFawwaz.AnimationUI;
using UnityEngine;

public class GameStateObserver : MonoBehaviour
{
    [SerializeField] AnimationUI _winShowAnimation;
    [SerializeField] AnimationUI _loseShowAnimation;
    [SerializeField] AnimationUI _pauseShowAnimation;
    [SerializeField] AnimationUI _pauseHideAnimation;

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
        } else if (state == GameState.Paused) {
            if (_pauseShowAnimation != null) {
                _pauseShowAnimation.Stop();
                _pauseShowAnimation.SetAllTweenTargetValueAsFrom();
                _pauseShowAnimation.Play();
            }
        } else if (state == GameState.Playing) {
            if (_pauseHideAnimation != null) {
                _pauseHideAnimation.Stop();
                _pauseHideAnimation.SetAllTweenTargetValueAsFrom();
                _pauseHideAnimation.Play();
            }
        }
    }
}
