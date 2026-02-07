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

[Serializable]
public class EnemyWave
{
    public float TimeThreshold;
    public int AdditionalHerds = 1;
}

[Serializable]
public class NeutralSpawnConfig
{
    public float TimeThreshold;
    public float SpawnInterval = 5f;
}

public class SignProgression : MonoBehaviour
{
    [Header("Sign Unlocks (conveyor belt cards)")]
    [SerializeField] List<SignUnlock> _unlocks = new();

    [Header("Random Event Unlocks")]
    [SerializeField] RandomEventScheduler _eventScheduler;
    [SerializeField] List<EventUnlock> _eventUnlocks = new();

    [Header("Enemy Waves")]
    [SerializeField] List<EnemyWave> _enemyWaves = new();

    [Header("Neutral Spawning")]
    [SerializeField] List<NeutralSpawnConfig> _neutralSpawnConfigs = new();

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
    int _nextEnemyWaveIndex;
    int _nextNeutralConfigIndex;
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
        _enemyWaves.Sort((a, b) => a.TimeThreshold.CompareTo(b.TimeThreshold));
        _neutralSpawnConfigs.Sort((a, b) => a.TimeThreshold.CompareTo(b.TimeThreshold));

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
        HandleEnemyWaves();
        HandleNeutralSpawning();
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

    void HandleEnemyWaves()
    {
        var spawner = HerdSpawner.Instance;
        if (spawner == null) return;

        while (_nextEnemyWaveIndex < _enemyWaves.Count && _elapsedTime >= _enemyWaves[_nextEnemyWaveIndex].TimeThreshold)
        {
            int count = _enemyWaves[_nextEnemyWaveIndex].AdditionalHerds;
            for (int i = 0; i < count; i++)
                spawner.SpawnAdditionalEnemyHerd();
            _nextEnemyWaveIndex++;
        }
    }

    void HandleNeutralSpawning()
    {
        var spawner = NeutralSpawner.Instance;
        if (spawner == null) return;

        while (_nextNeutralConfigIndex < _neutralSpawnConfigs.Count && _elapsedTime >= _neutralSpawnConfigs[_nextNeutralConfigIndex].TimeThreshold)
        {
            spawner.EnableSpawning();
            spawner.SetSpawnInterval(_neutralSpawnConfigs[_nextNeutralConfigIndex].SpawnInterval);
            _nextNeutralConfigIndex++;
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
