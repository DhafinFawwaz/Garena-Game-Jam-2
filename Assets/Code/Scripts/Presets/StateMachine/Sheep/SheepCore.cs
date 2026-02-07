using System;
using System.Collections.Generic;
using UnityEngine;

public class SheepCore : Core<SheepCore, SheepStates>, ISignInteractable
{
    public EntitySkin Skin;
    public EntityStats Stats;
    public EntityAttack Attack;
    public EntityDetector Detector;

    Herd _herd;
    public Herd Herd => _herd;

    public void SetHerd(Herd herd) {
        _herd = herd;
    }

    public void Init(HerdData data) {
        _moveSpeed = data.MoveSpeed;
    }

    public EntityVFX VFX;
    void Awake()
    {
        States = new SheepStates(this);
        CurrentState = States.Wander;
        CurrentState.StateEnter();
    }

    public override HitResult OnHurt(HitRequest hitRequest)
    {
        Attack.PlayHurtAnimation();
        Stats.CurrentHealth -= hitRequest.Damage;
        if(Stats.CurrentHealth <= 0) {
            Die();
        }
        return new HitResult();
    }

    public void OnEnterIdle()
    {
        Debug.Log("Enter Idle");
    }

    [SerializeField] Rigidbody2D _rb;
    public Rigidbody2D Rb => _rb;
    [SerializeField] float _moveSpeed = 5f;
    float _moveSpeedMultiplier = 1f;
    public float MoveSpeedMultiplier { get => _moveSpeedMultiplier; set => _moveSpeedMultiplier = value; }
    public float MoveSpeed => _moveSpeed * _moveSpeedMultiplier;

    public void DoFixedUpdate() {
        if(CurrentState == null) return;
        CurrentState.StateFixedUpdate();
    }
    public void DoUpdate() {
        if(CurrentState == null) return;
        CurrentState.StateUpdate();
    }

    [SerializeField] [ReadOnly] List<Sign> _currentSigns = new ();
    public List<Sign> CurrentSigns => _currentSigns;
    public Sign GetLatestSign() {
        if(_currentSigns.Count == 0) return null;
        return _currentSigns[_currentSigns.Count-1];
    }
    public void OnSignEnter(Sign sign) {
        _currentSigns.Add(sign);
        if(_currentSigns.Count >= 1) {
            SwitchState(States.Alert);
        }
    }

    public void OnSignExit(Sign sign) {
        _currentSigns.Remove(sign);
        if(_currentSigns.Count == 0) {
            _rb.linearVelocity = Vector2.zero;
            SwitchState(States.Wander);
        }
    }

    public Action<SheepCore> OnDeath;
    public void Die() {
        SwitchState(States.Death);
        OnDeath?.Invoke(this);
        Destroy(gameObject);
    }

    
}
