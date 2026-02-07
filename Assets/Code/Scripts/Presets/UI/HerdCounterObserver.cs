using System.Collections.Generic;
using UnityEngine;

public class HerdCounterObserver : MonoBehaviour
{
    [SerializeField] Counter _playerCounterPrefab;
    [SerializeField] Counter _enemyCounterPrefab;
    [SerializeField] Transform _counterParent;

    Dictionary<Herd, Counter> _counters = new Dictionary<Herd, Counter>();

    void OnEnable() {
        HerdSpawner.S_OnHerdSpawned += HandleHerdSpawned;
        Herd.S_OnMemberCountChanged += HandleMemberCountChanged;
    }

    void OnDisable() {
        HerdSpawner.S_OnHerdSpawned -= HandleHerdSpawned;
        Herd.S_OnMemberCountChanged -= HandleMemberCountChanged;
    }

    void HandleHerdSpawned(Herd herd) {
        Counter prefab = herd.IsPlayerHerd ? _playerCounterPrefab : _enemyCounterPrefab;
        Counter counter = Instantiate(prefab, _counterParent);
        counter.SetCount(herd.MemberCount);
        _counters[herd] = counter;
    }

    void HandleMemberCountChanged(MemberCountEvent e) {
        if (!_counters.ContainsKey(e.Herd)) return;
        _counters[e.Herd].SetCount(e.Count);
    }
}
