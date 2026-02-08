using System;
using UnityEngine;

public class NeutralSpawner : MonoBehaviour
{
    public static Action<SheepCore> S_OnNeutralSpawned;
    public static NeutralSpawner Instance { get; private set; }

    [Header("Prefab")]
    [SerializeField] SheepCore _neutralPrefab;

    [Header("Spawn Settings")]
    [SerializeField] Collider2D _spawnBoundary;
    [SerializeField] float _spawnPadding = 2f;
    [SerializeField] int _initialSpawnCount = 5;
    [SerializeField] float _spawnInterval = 10f;
    [SerializeField] int _maxNeutrals = 15;

    float _spawnTimer;
    [ReadOnly] public int AliveCount;
    bool _isActive;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnEnable()
    {
        GameManager.S_OnGameStateChanged += HandleGameStateChanged;
        HerdNeutralHolder.S_OnNeutralConverted += HandleNeutralConverted;
    }

    void OnDisable()
    {
        GameManager.S_OnGameStateChanged -= HandleGameStateChanged;
        HerdNeutralHolder.S_OnNeutralConverted -= HandleNeutralConverted;
    }

    void HandleGameStateChanged(GameState state)
    {
        if (state == GameState.Playing && !_isActive)
        {
            _isActive = true;
            SpawnInitialBatch();
        }
        else if (state != GameState.Playing)
        {
            _isActive = false;
        }
    }

    void SpawnInitialBatch()
    {
        for (int i = 0; i < _initialSpawnCount; i++)
        {
            SpawnNeutral();
        }
    }

    void Update()
    {
        if (!_isActive) return;
        if (AliveCount >= _maxNeutrals) return;

        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= _spawnInterval)
        {
            _spawnTimer = 0f;
            SpawnNeutral();
        }
    }

    void SpawnNeutral()
    {
        if (_neutralPrefab == null || _spawnBoundary == null) return;

        Vector2 pos = GetRandomPositionInBounds();
        SheepCore entity = Instantiate(_neutralPrefab, pos, Quaternion.identity);
        entity.gameObject.SetActive(true);
        entity.ConvertToNeutral();
        entity.PlayFallFromSkyAnimation();
        entity.OnDeath += HandleNeutralDeath;

        AliveCount++;
        S_OnNeutralSpawned?.Invoke(entity);
    }

    void HandleNeutralDeath(SheepCore entity)
    {
        entity.OnDeath -= HandleNeutralDeath;
        AliveCount = Mathf.Max(0, AliveCount - 1);
    }

    void HandleNeutralConverted(SheepCore entity)
    {
        entity.OnDeath -= HandleNeutralDeath;
        AliveCount = Mathf.Max(0, AliveCount - 1);
    }

    Vector2 GetRandomPositionInBounds()
    {
        Bounds bounds = _spawnBoundary.bounds;

        float minX = bounds.min.x + _spawnPadding;
        float maxX = bounds.max.x - _spawnPadding;
        float minY = bounds.min.y + _spawnPadding;
        float maxY = bounds.max.y - _spawnPadding;

        for (int attempt = 0; attempt < 50; attempt++)
        {
            Vector2 candidate = new Vector2(
                UnityEngine.Random.Range(minX, maxX),
                UnityEngine.Random.Range(minY, maxY)
            );

            if (_spawnBoundary.OverlapPoint(candidate))
                return candidate;
        }

        return (Vector2)bounds.center;
    }
}
