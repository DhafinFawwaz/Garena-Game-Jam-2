using System;
using System.Collections.Generic;
using UnityEngine;

public class HerdSpawner : MonoBehaviour
{
    public static Action<Herd> S_OnHerdSpawned;

    public static HerdSpawner Instance { get; private set; }

    [Header("Prefabs")]
    [SerializeField] SheepCore _entityPrefab;
    [SerializeField] Herd _herdPrefab;

    [Header("Herd Data")]
    [SerializeField] HerdData _playerHerdData;
    [SerializeField] List<HerdData> _enemyHerdDataList = new List<HerdData>();

    [Header("Spawn Settings")]
    [SerializeField] float _spawnPadding = 2f;
    [SerializeField] float _minHerdDistance = 5f;
    [SerializeField] float _bottomPadding = 4f;

    [SerializeField][ReadOnly] Herd _playerHerd;
    public Herd PlayerHerd { get => _playerHerd; }

    [SerializeField][ReadOnly] List<Herd> _enemyHerds = new List<Herd>();
    public List<Herd> EnemyHerds { get => _enemyHerds; }

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
        if (state == GameState.Playing)
        {
            SpawnAllHerds();
        }
    }

    void SpawnAllHerds()
    {
        List<Vector2> usedPositions = new List<Vector2>();

        Vector2 playerPos = GetRandomSpawnPosition(usedPositions);
        usedPositions.Add(playerPos);
        _playerHerd = SpawnHerd(_playerHerdData, playerPos, true);

        foreach (var enemyData in _enemyHerdDataList)
        {
            Vector2 enemyPos = GetRandomSpawnPosition(usedPositions);
            usedPositions.Add(enemyPos);
            var enemyHerd = SpawnHerd(enemyData, enemyPos, false);
            _enemyHerds.Add(enemyHerd);
        }
    }

    Vector2 GetRandomSpawnPosition(List<Vector2> usedPositions)
    {
        Camera cam = Camera.main;
        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;
        Vector2 camPos = cam.transform.position;

        float minX = camPos.x - camWidth + _spawnPadding;
        float maxX = camPos.x + camWidth - _spawnPadding;
        float minY = camPos.y - camHeight + _spawnPadding + _bottomPadding;
        float maxY = camPos.y + camHeight - _spawnPadding;

        for (int attempt = 0; attempt < 50; attempt++)
        {
            Vector2 candidate = new Vector2(
                UnityEngine.Random.Range(minX, maxX),
                UnityEngine.Random.Range(minY, maxY)
            );

            bool tooClose = false;
            foreach (var pos in usedPositions)
            {
                if (Vector2.Distance(candidate, pos) < _minHerdDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose) return candidate;
        }

        return new Vector2(
            UnityEngine.Random.Range(minX, maxX),
            UnityEngine.Random.Range(minY, maxY)
        );
    }

    Herd SpawnHerd(HerdData data, Vector2 position, bool isPlayer)
    {
        Herd herd = Instantiate(_herdPrefab, position, Quaternion.identity);
        herd.Init(data, isPlayer);

        for (int i = 0; i < data.InitialMemberCount; i++)
        {
            Vector2 spawnPos = position + UnityEngine.Random.insideUnitCircle * data.SpawnRadius;
            SheepCore entity = Instantiate(_entityPrefab, spawnPos, Quaternion.identity, herd.transform);
            entity.gameObject.SetActive(true);
            entity.Init(data);
            entity.Stats.CurrentTrust = UnityEngine.Random.Range(0f, 100f);
            herd.AddMember(entity);
        }

        S_OnHerdSpawned?.Invoke(herd);
        return herd;
    }

    public bool AreAllEnemyHerdsEmpty()
    {
        foreach (var herd in _enemyHerds)
        {
            if (herd != null && herd.MemberCount > 0) return false;
        }
        return true;
    }
}
