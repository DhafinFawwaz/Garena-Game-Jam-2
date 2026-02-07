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
    [SerializeField] float _spawnInterval = 5f;

    float _spawnTimer;
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
    }

    void OnDisable()
    {
        GameManager.S_OnGameStateChanged -= HandleGameStateChanged;
    }

    void HandleGameStateChanged(GameState state)
    {
        _isActive = state == GameState.Playing;
    }

    void Update()
    {
        if (!_isActive) return;

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
        entity.Stats.State = EntityType.Neutral;
        entity.PlayFallFromSkyAnimation();

        S_OnNeutralSpawned?.Invoke(entity);
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
