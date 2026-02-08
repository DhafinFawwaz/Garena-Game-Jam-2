using System;
using System.Collections.Generic;
using UnityEngine;

public class Herd : MonoBehaviour, IHungerable
{
    public static Action<HungerEvent> S_OnHungerChanged;
    public static Action<HungerEvent> S_OnHungerDepleted;
    public static Action<Herd> S_OnMemberConsumed;
    public static Action<Herd> S_OnHerdEmpty;
    public static Action<MemberCountEvent> S_OnMemberCountChanged;

    [SerializeField][ReadOnly] bool _isPlayerHerd;
    public bool IsPlayerHerd { get => _isPlayerHerd; }

    [SerializeField][ReadOnly] string _herdName;
    public string HerdName { get => _herdName; }

    float _maxHunger;
    public float MaxHunger { get => _maxHunger; }

    float _hungerLevel;
    public float HungerLevel { get => _hungerLevel; }

    float _hungerDecayRate;
    float _hungerDecayMultiplier = 1f;
    public float HungerDecayMultiplier { get => _hungerDecayMultiplier; set => _hungerDecayMultiplier = value; }

    private int _teamID = -1;
    public int TeamID { get => _teamID; }

    private static List<int> _teams = new();

    [SerializeField][ReadOnly] List<SheepCore> _members = new List<SheepCore>();
    public List<SheepCore> Members { get => _members; }
    public int MemberCount { get => _members.Count; }

    public event Action OnHungerDepleted;
    public Action<Herd> OnHerdEmpty;

    public void Init(HerdData data, bool isPlayerHerd)
    {
        _isPlayerHerd = isPlayerHerd;
        _herdName = data.HerdName;
        _maxHunger = data.MaxHunger;
        _hungerLevel = data.MaxHunger;
        _hungerDecayRate = data.HungerDecayRate;

        _teamID = UnityEngine.Random.Range(0, 100);
        while (_teams.Contains(_teamID))
        {
            _teamID = UnityEngine.Random.Range(0, 100);
        }
        _teams.Add(_teamID);
    }

    public void AddMember(SheepCore entity)
    {
        _members.Add(entity);
        entity.SetHerd(this);
        entity.OnDeath += HandleMemberDeath;
        BroadcastMemberCountChanged();
    }

    public void RemoveMember(SheepCore entity)
    {
        entity.OnDeath -= HandleMemberDeath;
        _members.Remove(entity);
        BroadcastMemberCountChanged();
        if (_members.Count == 0)
        {
            OnHerdEmpty?.Invoke(this);
            S_OnHerdEmpty?.Invoke(this);
        }
    }

    void HandleMemberDeath(SheepCore entity)
    {
        _members.Remove(entity);
        BroadcastMemberCountChanged();
        if (_members.Count == 0)
        {

            Debug.Log("[HERD] : Herd is Empty");
            OnHerdEmpty?.Invoke(this);
            S_OnHerdEmpty?.Invoke(this);
        }
    }

    public void DoUpdate()
    {
        if (_members.Count == 0) return;
        DecreaseHunger(_hungerDecayRate * _hungerDecayMultiplier * Time.deltaTime);
    }

    public void IncreaseHunger(float amount)
    {
        _hungerLevel = Mathf.Clamp(_hungerLevel + amount, 0f, _maxHunger);
        BroadcastHungerChanged();
    }

    public void DecreaseHunger(float amount)
    {
        _hungerLevel = Mathf.Clamp(_hungerLevel - amount, 0f, _maxHunger);
        BroadcastHungerChanged();
        if (_hungerLevel <= 0f)
        {
            OnHungerDepleted?.Invoke();
            S_OnHungerDepleted?.Invoke(new HungerEvent { Herd = this, HungerNormalized = 0f });
            ConsumeOneMember();
        }
    }

    void BroadcastMemberCountChanged()
    {
        S_OnMemberCountChanged?.Invoke(new MemberCountEvent { Herd = this, Count = _members.Count });
    }

    void BroadcastHungerChanged()
    {
        S_OnHungerChanged?.Invoke(new HungerEvent { Herd = this, HungerNormalized = CalculateHungerNormalized() });
    }

    void ConsumeOneMember()
    {
        if (_members.Count == 0) return;
        var victim = _members[_members.Count - 1];
        RemoveMember(victim);
        victim.Die();
        _hungerLevel = _maxHunger;
        S_OnMemberConsumed?.Invoke(this);
        BroadcastHungerChanged();
    }

    public float CalculateHungerNormalized()
    {
        return _hungerLevel / _maxHunger;
    }

    [Header("Recycle Settings")]
    [SerializeField] float _recycleRatio = 0.2f;
    [SerializeField] float _recycleDamageBonus = 5f;
    [SerializeField] float _recycleHealthBonus = 20f;

    public void Recycle()
    {
        int sacrificeCount = Mathf.Max(1, Mathf.FloorToInt(_members.Count * _recycleRatio));
        if (_members.Count <= sacrificeCount) return;

        for (int i = 0; i < sacrificeCount; i++)
        {
            var victim = _members[_members.Count - 1];
            RemoveMember(victim);
            victim.Die();
        }

        foreach (var member in _members)
        {
            member.Stats.AttackDamage += _recycleDamageBonus;
            member.Stats.MaxHealth += _recycleHealthBonus;
            member.Stats.CurrentHealth = Mathf.Min(member.Stats.CurrentHealth + _recycleHealthBonus, member.Stats.MaxHealth);
        }

        _hungerLevel = _maxHunger;
        BroadcastHungerChanged();
    }
}
