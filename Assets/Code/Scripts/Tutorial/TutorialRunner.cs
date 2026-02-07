using System;
using System.Collections;
using UnityEngine;

public class TutorialRunner : MonoBehaviour
{
    public static Action S_OnTutorialStarted;
    public static Action S_OnTutorialEnded;

    [SerializeField] Tutorial _tutorial;
    [SerializeField] TutorialPopup _popup;
    [SerializeField] bool _startOnGamePlaying = true;

    int _currentStepIndex;
    bool _isRunning;

    void OnEnable()
    {
        if (_startOnGamePlaying)
            GameManager.S_OnGameStateChanged += HandleGameStateChanged;
    }

    void OnDisable()
    {
        if (_startOnGamePlaying)
            GameManager.S_OnGameStateChanged -= HandleGameStateChanged;
    }

    void HandleGameStateChanged(GameState state)
    {
        if (state == GameState.Playing)
            StartTutorial();
    }

    public void StartTutorial()
    {
        if (_tutorial == null || _popup == null) return;
        if (_tutorial.ShowOnlyOnce && _tutorial.HasBeenCompleted()) return;
        if (_isRunning) return;

        _isRunning = true;
        _currentStepIndex = 0;
        S_OnTutorialStarted?.Invoke();
        _popup.OnNextClicked += AdvanceStep;
        ShowCurrentStep();
    }

    void ShowCurrentStep()
    {
        if (_currentStepIndex >= _tutorial.Steps.Count)
        {
            EndTutorial();
            return;
        }

        TutorialStep step = _tutorial.Steps[_currentStepIndex];
        bool isLast = _currentStepIndex == _tutorial.Steps.Count - 1;

        if (step.PauseGame)
            Time.timeScale = 0f;

        _popup.Show(step, isLast);

        if (step.AutoAdvanceDelay > 0f)
            StartCoroutine(AutoAdvance(step.AutoAdvanceDelay));
    }

    IEnumerator AutoAdvance(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        AdvanceStep();
    }

    void AdvanceStep()
    {
        TutorialStep currentStep = _tutorial.Steps[_currentStepIndex];
        if (currentStep.PauseGame)
            Time.timeScale = 1f;

        _currentStepIndex++;
        if (_currentStepIndex >= _tutorial.Steps.Count)
        {
            EndTutorial();
        }
        else
        {
            ShowCurrentStep();
        }
    }

    void EndTutorial()
    {
        _popup.OnNextClicked -= AdvanceStep;
        _popup.Hide();
        Time.timeScale = 1f;
        _isRunning = false;
        _tutorial.MarkCompleted();
        S_OnTutorialEnded?.Invoke();
    }
}
