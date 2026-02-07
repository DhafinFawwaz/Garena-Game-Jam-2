using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SignUnlock
{
    public float TimeThreshold;
    public Sign SignPrefab;
}

[Serializable]
public class EventUnlock
{
    public float TimeThreshold;
    public GameEvent Event;
}

public class SignProgression : MonoBehaviour
{
    [Header("Sign Unlocks (conveyor belt cards)")]
    [SerializeField] List<SignUnlock> _unlocks = new();

    [Header("Random Event Unlocks")]
    [SerializeField] RandomEventScheduler _eventScheduler;
    [SerializeField] List<EventUnlock> _eventUnlocks = new();

    [Header("Conveyor Belt Scaling")]
    [SerializeField] float _initialSpawnInterval = 3f;
    [SerializeField] float _minSpawnInterval = 0.8f;
    [SerializeField] float _spawnIntervalDecayRate = 0.02f;
    [SerializeField] int _initialMaxSigns = 3;
    [SerializeField] int _finalMaxSigns = 8;
    [SerializeField] float _maxSignsScaleTime = 120f;

    float _elapsedTime;
    int _nextUnlockIndex;
    int _nextEventUnlockIndex;
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
        _eventUnlocks.Sort((a, b) => a.TimeThreshold.CompareTo(b.TimeThreshold));

        if (ConveyorBelt.Instance != null)
        {
            ConveyorBelt.Instance.SetSpawnInterval(_initialSpawnInterval);
            ConveyorBelt.Instance.SetMaxActiveSigns(_initialMaxSigns);
        }
    }

    void Update()
    {
        if (!_isActive) return;

        _elapsedTime += Time.deltaTime;

        HandleSignUnlocks();
        HandleEventUnlocks();
        HandleSpawnIntervalDecay();
        HandleMaxSignsScaling();
    }

    void HandleSignUnlocks()
    {
        if (ConveyorBelt.Instance == null) return;

        while (_nextUnlockIndex < _unlocks.Count && _elapsedTime >= _unlocks[_nextUnlockIndex].TimeThreshold)
        {
            ConveyorBelt.Instance.AddSignPrefab(_unlocks[_nextUnlockIndex].SignPrefab);
            _nextUnlockIndex++;
        }
    }

    void HandleEventUnlocks()
    {
        if (_eventScheduler == null) return;

        while (_nextEventUnlockIndex < _eventUnlocks.Count && _elapsedTime >= _eventUnlocks[_nextEventUnlockIndex].TimeThreshold)
        {
            _eventScheduler.AddEvent(_eventUnlocks[_nextEventUnlockIndex].Event);
            _nextEventUnlockIndex++;
        }
    }

    void HandleSpawnIntervalDecay()
    {
        if (ConveyorBelt.Instance == null) return;

        float interval = Mathf.Max(_minSpawnInterval, _initialSpawnInterval - (_elapsedTime * _spawnIntervalDecayRate));
        ConveyorBelt.Instance.SetSpawnInterval(interval);
    }

    void HandleMaxSignsScaling()
    {
        if (ConveyorBelt.Instance == null) return;

        float t = Mathf.Clamp01(_elapsedTime / _maxSignsScaleTime);
        int maxSigns = Mathf.RoundToInt(Mathf.Lerp(_initialMaxSigns, _finalMaxSigns, t));
        ConveyorBelt.Instance.SetMaxActiveSigns(maxSigns);
    }
}
