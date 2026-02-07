using System.Collections.Generic;
using UnityEngine;

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

    [Header("Sign Spawning")]
    [SerializeField] List<Sign> _signPrefabs = new();
    public List<Sign> SignPrefabs => _signPrefabs;

    [SerializeField] int _spawnCount = 1;
    public int SpawnCount => _spawnCount;

    [SerializeField] float _signSpawnRadius = 5f;
    public float SignSpawnRadius => _signSpawnRadius;
}
