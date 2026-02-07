using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEventScheduler : MonoBehaviour
{
    public static Action<GameEvent> S_OnEventTriggered;

    [Header("Event Pool")]
    [SerializeField][ReadOnly] List<GameEvent> _eventPool = new();

    [Header("Scheduling")]
    [SerializeField] float _initialInterval = 30f;
    [SerializeField] float _minInterval = 10f;
    [SerializeField] float _intervalDecayRate = 0.5f;

    [Header("Multi-Event Scaling")]
    [SerializeField] int _initialEventCount = 1;
    [SerializeField] int _maxEventCount = 3;
    [SerializeField] float _eventCountScaleTime = 180f;

    [Header("Spawn Boundary")]
    [SerializeField] Collider2D _spawnBoundary;
    [SerializeField] float _spawnPadding = 2f;

    [Header("Display")]
    [SerializeField] PopUp _eventPopup;
    [SerializeField] float _popupDuration = 3f;

    float _elapsedTime;
    float _nextEventTime;
    bool _isActive;

    void Start()
    {
        _nextEventTime = _initialInterval;
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
        if (!_isActive || _eventPool.Count == 0) return;

        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _nextEventTime)
        {
            TriggerRandomEvent();
            ScheduleNextEvent();
        }
    }

    void TriggerRandomEvent()
    {
        if (_eventPool.Count == 0) return;

        int count = GetCurrentEventCount();
        for (int e = 0; e < count; e++)
        {
            int index = UnityEngine.Random.Range(0, _eventPool.Count);
            GameEvent evt = _eventPool[index];

            S_OnEventTriggered?.Invoke(evt);
            SpawnSigns(evt);

            if (_eventPopup != null && e == 0)
            {
                _eventPopup.SetData(evt.EventName, evt.Description);
                _eventPopup.Show();
                StartCoroutine(HidePopupAfterDelay());
            }
        }
    }

    int GetCurrentEventCount()
    {
        float t = Mathf.Clamp01(_elapsedTime / _eventCountScaleTime);
        return Mathf.RoundToInt(Mathf.Lerp(_initialEventCount, _maxEventCount, t));
    }

    IEnumerator HidePopupAfterDelay()
    {
        yield return new WaitForSeconds(_popupDuration);
        if (_eventPopup != null)
            _eventPopup.Hide();
    }

    void ScheduleNextEvent()
    {
        float currentInterval = Mathf.Max(_minInterval, _initialInterval - (_elapsedTime * _intervalDecayRate));
        _nextEventTime = _elapsedTime + currentInterval;
    }

    void SpawnSigns(GameEvent evt)
    {
        if (evt.SignPrefabs == null || evt.SignPrefabs.Count == 0) return;

        for (int i = 0; i < evt.SpawnCount; i++)
        {
            Sign prefab = evt.SignPrefabs[UnityEngine.Random.Range(0, evt.SignPrefabs.Count)];
            if (prefab == null) continue;

            Vector2 pos = GetRandomPositionInBounds();
            Instantiate(prefab, pos, Quaternion.identity);
        }
    }

    Vector2 GetRandomPositionInBounds()
    {
        if (_spawnBoundary == null)
            return Vector2.zero;

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

    public void AddEvent(GameEvent evt)
    {
        if (_eventPool.Contains(evt)) return;

        bool wasEmpty = _eventPool.Count == 0;
        _eventPool.Add(evt);

        if (wasEmpty)
        {
            _elapsedTime = 0f;
            _nextEventTime = _initialInterval;
        }
    }
}
