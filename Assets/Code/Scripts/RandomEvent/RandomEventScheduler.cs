using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEventScheduler : MonoBehaviour
{
    public static Action<GameEvent> S_OnEventTriggered;

    [Header("Event Pool")]
    [SerializeField] List<GameEvent> _eventPool = new();

    [Header("Scheduling")]
    [SerializeField] float _initialInterval = 30f;
    [SerializeField] float _minInterval = 10f;
    [SerializeField] float _intervalDecayRate = 0.5f;

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
        if (!_isActive) return;

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

        int index = UnityEngine.Random.Range(0, _eventPool.Count);
        GameEvent evt = _eventPool[index];

        S_OnEventTriggered?.Invoke(evt);

        if (_eventPopup != null)
        {
            _eventPopup.SetData(evt.EventName, evt.Description);
            _eventPopup.Show();
            StartCoroutine(HidePopupAfterDelay());
        }

        ApplyEffect(evt);
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

    void ApplyEffect(GameEvent evt)
    {
        StartCoroutine(ApplyTimedEffect(evt));
    }

    IEnumerator ApplyTimedEffect(GameEvent evt)
    {
        var spawner = HerdSpawner.Instance;
        if (spawner == null) yield break;

        List<Herd> targetHerds = new List<Herd>();
        if (evt.AffectsPlayerOnly)
        {
            if (spawner.PlayerHerd != null)
                targetHerds.Add(spawner.PlayerHerd);
        }
        else
        {
            if (spawner.PlayerHerd != null)
                targetHerds.Add(spawner.PlayerHerd);
            targetHerds.AddRange(spawner.EnemyHerds);
        }

        switch (evt.Effect)
        {
            case GameEventEffect.HungerDecayMultiplier:
                foreach (var herd in targetHerds)
                    if (herd != null) herd.HungerDecayMultiplier = evt.EffectValue;
                yield return new WaitForSeconds(evt.Duration);
                foreach (var herd in targetHerds)
                    if (herd != null) herd.HungerDecayMultiplier = 1f;
                break;

            case GameEventEffect.SpawnIntervalMultiplier:
                if (ConveyorBelt.Instance != null)
                {
                    ConveyorBelt.Instance.SetSpawnIntervalMultiplier(evt.EffectValue);
                    yield return new WaitForSeconds(evt.Duration);
                    ConveyorBelt.Instance.SetSpawnIntervalMultiplier(1f);
                }
                break;

            case GameEventEffect.InstantHungerDrain:
                foreach (var herd in targetHerds)
                    if (herd != null) herd.DecreaseHunger(evt.EffectValue);
                break;

            case GameEventEffect.InstantHungerRestore:
                foreach (var herd in targetHerds)
                    if (herd != null) herd.IncreaseHunger(evt.EffectValue);
                break;

            case GameEventEffect.MoveSpeedMultiplier:
                foreach (var herd in targetHerds)
                {
                    if (herd == null) continue;
                    foreach (var member in herd.Members)
                        if (member != null) member.MoveSpeedMultiplier = evt.EffectValue;
                }
                yield return new WaitForSeconds(evt.Duration);
                foreach (var herd in targetHerds)
                {
                    if (herd == null) continue;
                    foreach (var member in herd.Members)
                        if (member != null) member.MoveSpeedMultiplier = 1f;
                }
                break;
        }
    }
}
