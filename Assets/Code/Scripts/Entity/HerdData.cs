using UnityEngine;

[CreateAssetMenu(fileName = "NewHerdData", menuName = "Game/HerdData")]
public class HerdData : ScriptableObject
{
    [Header("Herd Info")]
    [SerializeField] string _herdName = "Herd";
    public string HerdName { get => _herdName; }

    [SerializeField] Color _herdColor = Color.white;
    public Color HerdColor { get => _herdColor; }

    [Header("Spawn Settings")]
    [SerializeField] int _initialMemberCount = 5;
    public int InitialMemberCount { get => _initialMemberCount; }

    [SerializeField] float _spawnRadius = 3f;
    public float SpawnRadius { get => _spawnRadius; }

    [Header("Hunger Settings")]
    [SerializeField] float _maxHunger = 100f;
    public float MaxHunger { get => _maxHunger; }

    [SerializeField] float _hungerDecayRate = 1f;
    public float HungerDecayRate { get => _hungerDecayRate; }

    [Header("Entity Settings")]
    [SerializeField] float _moveSpeed = 5f;
    public float MoveSpeed { get => _moveSpeed; }

    [SerializeField] float _initialTrustLevel = 0.5f;
    public float InitialTrustLevel { get => _initialTrustLevel; }
}
