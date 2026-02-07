using System;
using System.Collections.Generic;
using UnityEngine;

public class Herd : MonoBehaviour, IHungerable
{
    public static Action<HungerEvent> S_OnHungerChanged;
    public static Action<HungerEvent> S_OnHungerDepleted;
    public static Action<Herd> S_OnMemberConsumed;
    public static Action<Herd> S_OnHerdEmpty;

    [SerializeField] [ReadOnly] bool _isPlayerHerd;
    public bool IsPlayerHerd { get => _isPlayerHerd; }

    [SerializeField] [ReadOnly] string _herdName;
    public string HerdName { get => _herdName; }

    float _maxHunger;
    public float MaxHunger { get => _maxHunger; }

    float _hungerLevel;
    public float HungerLevel { get => _hungerLevel; }

    float _hungerDecayRate;

    [SerializeField] [ReadOnly] List<Entity> _members = new List<Entity>();
    public List<Entity> Members { get => _members; }
    public int MemberCount { get => _members.Count; }

    public event Action OnHungerDepleted;
    public Action<Herd> OnHerdEmpty;

    public void Init(HerdData data, bool isPlayerHerd) {
        _isPlayerHerd = isPlayerHerd;
        _herdName = data.HerdName;
        _maxHunger = data.MaxHunger;
        _hungerLevel = data.MaxHunger;
        _hungerDecayRate = data.HungerDecayRate;
    }

    public void AddMember(Entity entity) {
        _members.Add(entity);
        entity.SetHerd(this);
        entity.OnDeath += HandleMemberDeath;
    }

    public void RemoveMember(Entity entity) {
        entity.OnDeath -= HandleMemberDeath;
        _members.Remove(entity);
        if(_members.Count == 0) {
            OnHerdEmpty?.Invoke(this);
            S_OnHerdEmpty?.Invoke(this);
        }
    }

    void HandleMemberDeath(Entity entity) {
        _members.Remove(entity);
        if(_members.Count == 0) {
            OnHerdEmpty?.Invoke(this);
            S_OnHerdEmpty?.Invoke(this);
        }
    }

    public void DoUpdate() {
        if(_members.Count == 0) return;
        DecreaseHunger(_hungerDecayRate * Time.deltaTime);
    }

    public void IncreaseHunger(float amount) {
        _hungerLevel = Mathf.Clamp(_hungerLevel + amount, 0f, _maxHunger);
        BroadcastHungerChanged();
    }

    public void DecreaseHunger(float amount) {
        _hungerLevel = Mathf.Clamp(_hungerLevel - amount, 0f, _maxHunger);
        BroadcastHungerChanged();
        if(_hungerLevel <= 0f) {
            OnHungerDepleted?.Invoke();
            S_OnHungerDepleted?.Invoke(new HungerEvent { Herd = this, HungerNormalized = 0f });
            ConsumeOneMember();
        }
    }

    void BroadcastHungerChanged() {
        S_OnHungerChanged?.Invoke(new HungerEvent { Herd = this, HungerNormalized = CalculateHungerNormalized() });
    }

    void ConsumeOneMember() {
        if(_members.Count == 0) return;
        var victim = _members[_members.Count - 1];
        RemoveMember(victim);
        victim.Die();
        _hungerLevel = _maxHunger;
        S_OnMemberConsumed?.Invoke(this);
        BroadcastHungerChanged();
    }

    public float CalculateHungerNormalized() {
        return _hungerLevel / _maxHunger;
    }
}
