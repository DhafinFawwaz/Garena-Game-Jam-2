using System;
using UnityEngine;

public enum GameState
{
    Idle,
    Playing,
    Paused,
    Win,
    Lose
}

public class GameManager : MonoBehaviour
{
    public static Action<GameState> S_OnGameStateChanged;

    [SerializeField][ReadOnly] GameState _currentState = GameState.Idle;
    public GameState CurrentState { get => _currentState; }

    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        GameManager.Instance.StartGame();
    }

    void OnEnable()
    {
        Herd.S_OnHerdEmpty += HandleHerdEmpty;
    }

    void OnDisable()
    {
        Herd.S_OnHerdEmpty -= HandleHerdEmpty;
    }

    public void StartGame()
    {
        SetState(GameState.Playing);
    }

    public void SetState(GameState newState)
    {
        if (_currentState == newState) return;
        _currentState = newState;
        S_OnGameStateChanged?.Invoke(_currentState);
    }

    GameState _stateBeforePause;

    public void PauseGame()
    {
        if (_currentState != GameState.Playing) return;
        _stateBeforePause = _currentState;
        Time.timeScale = 0f;
        SetState(GameState.Paused);
    }

    public void ResumeGame()
    {
        if (_currentState != GameState.Paused) return;
        Time.timeScale = 1f;
        SetState(_stateBeforePause);
    }

    void HandleHerdEmpty(Herd herd)
    {
        if (_currentState != GameState.Playing) return;
        if (herd.IsPlayerHerd)
        {
            SetState(GameState.Lose);
        }
        else
        {
            CheckWinCondition();
        }
    }

    void CheckWinCondition()
    {
        var herds = HerdSpawner.Instance;
        if (herds == null) return;
        if (herds.AreAllEnemyHerdsEmpty())
        {
            SetState(GameState.Win);
        }
    }
}
