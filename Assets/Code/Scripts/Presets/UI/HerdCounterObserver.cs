using UnityEngine;

public class HerdCounterObserver : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] Counter _playerCounter;

    [Header("Enemies (fixed 3)")]
    [SerializeField] Counter _enemyCounter0;
    [SerializeField] Counter _enemyCounter1;
    [SerializeField] Counter _enemyCounter2;

    [Header("Neutral")]
    [SerializeField] Counter _neutralCounter;

    int _neutralCount;

    void OnEnable()
    {
        HerdSpawner.S_OnHerdSpawned += HandleHerdSpawned;
        Herd.S_OnMemberCountChanged += HandleMemberCountChanged;
        NeutralSpawner.S_OnNeutralSpawned += HandleNeutralSpawned;
        HerdNeutralHolder.S_OnNeutralConverted += HandleNeutralConverted;
    }

    void OnDisable()
    {
        HerdSpawner.S_OnHerdSpawned -= HandleHerdSpawned;
        Herd.S_OnMemberCountChanged -= HandleMemberCountChanged;
        NeutralSpawner.S_OnNeutralSpawned -= HandleNeutralSpawned;
        HerdNeutralHolder.S_OnNeutralConverted -= HandleNeutralConverted;
    }

    void HandleHerdSpawned(Herd herd)
    {
        if (herd.IsPlayerHerd)
        {
            _playerCounter.SetCount(herd.MemberCount);
        }
        else
        {
            var spawner = HerdSpawner.Instance;
            if (spawner == null) return;

            int index = spawner.EnemyHerds.IndexOf(herd);
            Counter counter = GetEnemyCounter(index);
            if (counter != null)
                counter.SetCount(herd.MemberCount);
        }
    }

    void HandleMemberCountChanged(MemberCountEvent e)
    {
        if (e.Herd.IsPlayerHerd)
        {
            _playerCounter.SetCount(e.Count);
            return;
        }

        var spawner = HerdSpawner.Instance;
        if (spawner == null) return;

        int index = spawner.EnemyHerds.IndexOf(e.Herd);
        Counter counter = GetEnemyCounter(index);
        if (counter != null)
            counter.SetCount(e.Count);
    }

    void HandleNeutralSpawned(SheepCore entity)
    {
        _neutralCount++;
        _neutralCounter.SetCount(_neutralCount);
        entity.OnDeath += HandleNeutralDeath;
    }

    void HandleNeutralDeath(SheepCore entity)
    {
        entity.OnDeath -= HandleNeutralDeath;
        _neutralCount = Mathf.Max(0, _neutralCount - 1);
        _neutralCounter.SetCount(_neutralCount);
    }

    void HandleNeutralConverted(SheepCore entity)
    {
        entity.OnDeath -= HandleNeutralDeath;
        _neutralCount = Mathf.Max(0, _neutralCount - 1);
        _neutralCounter.SetCount(_neutralCount);
    }

    Counter GetEnemyCounter(int index)
    {
        return index switch
        {
            0 => _enemyCounter0,
            1 => _enemyCounter1,
            2 => _enemyCounter2,
            _ => null
        };
    }
}
