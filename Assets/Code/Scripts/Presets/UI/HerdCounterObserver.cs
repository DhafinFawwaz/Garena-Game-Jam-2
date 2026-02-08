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

    void OnEnable()
    {
        HerdSpawner.S_OnHerdSpawned += HandleHerdSpawned;
        Herd.S_OnMemberCountChanged += HandleMemberCountChanged;
    }

    void OnDisable()
    {
        HerdSpawner.S_OnHerdSpawned -= HandleHerdSpawned;
        Herd.S_OnMemberCountChanged -= HandleMemberCountChanged;
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

    void Update()
    {
        if (NeutralSpawner.Instance != null)
            _neutralCounter.SetCount(NeutralSpawner.Instance.AliveCount);
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
