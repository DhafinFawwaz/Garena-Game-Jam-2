using System;
using System.Collections.Generic;
using UnityEngine;

public enum GameEventEffect
{
    HungerDecayMultiplier,
    SpawnIntervalMultiplier,
    MoveSpeedMultiplier,
    InstantHungerDrain,
    InstantHungerRestore,
    SpawnSign,
    SpawnEnemyHerd
}

[CreateAssetMenu(fileName = "NewGameEvent", menuName = "Game/GameEvent")]
public class GameEvent : ScriptableObject
{
    [Header("Display")]
    [SerializeField] string _eventName = "Event";
    public string EventName => _eventName;

    [SerializeField] [TextArea] string _description = "Something happened!";
    public string Description => _description;

    [SerializeField] Sprite _icon;
    public Sprite Icon => _icon;

    [Header("Effect")]
    [SerializeField] GameEventEffect _effect;
    public GameEventEffect Effect => _effect;

    [SerializeField] float _effectValue = 1f;
    public float EffectValue => _effectValue;

    [SerializeField] float _duration = 10f;
    public float Duration => _duration;

    [SerializeField] bool _affectsPlayerOnly = false;
    public bool AffectsPlayerOnly => _affectsPlayerOnly;

    [Header("Sign Spawning (for SpawnSign effect)")]
    [SerializeField] List<Sign> _signPrefabs = new();
    public List<Sign> SignPrefabs => _signPrefabs;

    [SerializeField] int _spawnCount = 1;
    public int SpawnCount => _spawnCount;

    [SerializeField] float _signSpawnRadius = 5f;
    public float SignSpawnRadius => _signSpawnRadius;
}
