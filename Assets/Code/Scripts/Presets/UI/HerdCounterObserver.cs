using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HerdCounterObserver : MonoBehaviour
{
    [SerializeField] TMP_Text _counterPrefab;
    [SerializeField] Transform _counterParent;

    Dictionary<Herd, TMP_Text> _counters = new Dictionary<Herd, TMP_Text>();

    void OnEnable() {
        HerdSpawner.S_OnHerdSpawned += HandleHerdSpawned;
        Herd.S_OnMemberCountChanged += HandleMemberCountChanged;
        Herd.S_OnHerdEmpty += HandleHerdEmpty;
    }

    void OnDisable() {
        HerdSpawner.S_OnHerdSpawned -= HandleHerdSpawned;
        Herd.S_OnMemberCountChanged -= HandleMemberCountChanged;
        Herd.S_OnHerdEmpty -= HandleHerdEmpty;
    }

    void HandleHerdSpawned(Herd herd) {
        TMP_Text counter = Instantiate(_counterPrefab, _counterParent);
        counter.text = herd.HerdName + ": " + herd.MemberCount;
        _counters[herd] = counter;
    }

    void HandleMemberCountChanged(MemberCountEvent e) {
        if (!_counters.ContainsKey(e.Herd)) return;
        _counters[e.Herd].text = e.Herd.HerdName + ": " + e.Count;
    }

    void HandleHerdEmpty(Herd herd) {
        if (!_counters.ContainsKey(herd)) return;
        _counters[herd].text = herd.HerdName + ": 0";
    }
}
