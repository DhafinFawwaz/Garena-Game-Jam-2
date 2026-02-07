using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SignUnlock
{
    public float TimeThreshold;
    public SignDragable SignPrefab;
}

public class SignProgression : MonoBehaviour
{
    [SerializeField] List<SignUnlock> _unlocks = new();
    [SerializeField] float _spawnIntervalDecayRate = 0.05f;
    [SerializeField] float _minSpawnInterval = 0.5f;
    [SerializeField] float _initialSpawnInterval = 2f;

    float _elapsedTime;
    int _nextUnlockIndex;
    bool _isActive;

    void OnEnable()
    {
        GameManager.S_OnGameStateChanged += HandleGameStateChanged;
    }

    void OnDisable()
    {
        GameManager.S_OnGameStateChanged -= HandleGameStateChanged;
    }

    void HandleGameStateChanged(GameState state)
    {
        _isActive = state == GameState.Playing;
    }

    void Start()
    {
        _unlocks.Sort((a, b) => a.TimeThreshold.CompareTo(b.TimeThreshold));

        if (ConveyorBelt.Instance != null)
            ConveyorBelt.Instance.SetSpawnInterval(_initialSpawnInterval);
    }

    void Update()
    {
        if (!_isActive) return;

        _elapsedTime += Time.deltaTime;

        HandleUnlocks();
        HandleSpawnIntervalDecay();
    }

    void HandleUnlocks()
    {
        if (ConveyorBelt.Instance == null) return;

        while (_nextUnlockIndex < _unlocks.Count && _elapsedTime >= _unlocks[_nextUnlockIndex].TimeThreshold)
        {
            ConveyorBelt.Instance.AddSignPrefab(_unlocks[_nextUnlockIndex].SignPrefab);
            _nextUnlockIndex++;
        }
    }

    void HandleSpawnIntervalDecay()
    {
        if (ConveyorBelt.Instance == null) return;

        float interval = Mathf.Max(_minSpawnInterval, _initialSpawnInterval - (_elapsedTime * _spawnIntervalDecayRate));
        ConveyorBelt.Instance.SetSpawnInterval(interval);
    }
}
