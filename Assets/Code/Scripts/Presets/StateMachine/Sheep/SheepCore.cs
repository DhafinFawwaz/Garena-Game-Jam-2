using System;
using System.Collections.Generic;
using DhafinFawwaz.AnimationUI;
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
            if(hitRequest.Element == Element.Bomb) {
                ApplyTrustToWitness();
            }
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

        Stats.CurrentHunger -= Stats.HungerDecreasePerSecond * Time.deltaTime;
    }

    [SerializeField] [ReadOnly] List<Sign> _currentSigns = new ();
    public List<Sign> CurrentSigns => _currentSigns;
    public Sign GetLatestSign() {
        if(_currentSigns.Count == 0) return null;
        return _currentSigns[_currentSigns.Count-1];
    }



    Sign _lastSignThatCausedAlert = null;
    public Sign LastSignThatCausedAlert { get => _lastSignThatCausedAlert; set => _lastSignThatCausedAlert = value; }
    public void OnSignEnter(Sign sign) {
        _currentSigns.Add(sign);
        if(_currentSigns.Count >= 1) {
            // if alert twice with same sign, ignore
            if(_lastSignThatCausedAlert == sign) return;
            _lastSignThatCausedAlert = sign;
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
    bool _isDead = false;
    public void Die() {
        if(_isDead) return;
        _isDead = true;
        SetActiveCore(false);
        SwitchState(States.Death);
        OnDeath?.Invoke(this);
        Skin.PlayDeathAnimation();
        Destroy(gameObject, 5f);
    }


    [Header("Witness Trust Settings")]
    [SerializeField] float _trustOnWitnessDeath = 10f; // TODO: edit this
    [SerializeField] float _witnessRadius = 5f;
    void ApplyTrustToWitness() {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _witnessRadius);
        foreach(var hit in hits) {
            if(hit.TryGetComponent<SheepCore>(out var playerCore)) {
                playerCore.IncreaseTrust(_trustOnWitnessDeath);
            }
        }
    }
    public void IncreaseTrust(float amount) {
        Stats.CurrentTrust += amount;
        // TODO: play some VFX or SFX
    }
    public void DecreaseTrust(float amount) { // called when friendly sheep die
        Stats.CurrentTrust -= amount;
    }

    public void ConvertToEnemy() {
        Stats.State = EntityType.Enemy;
        // TODO: change visual
    }
    public void ConvertToFriendly() {
        Stats.State = EntityType.Friendly;
        // TODO: change visual
    }

    [SerializeField] Collider2D[] _col;
    [SerializeField] GameObject[] _objs;
    public void SetActiveCore(bool isActive) {
        if(isActive) {
            _rb.bodyType = RigidbodyType2D.Dynamic;
            foreach(var col in _col) {
                col.enabled = true;
            }
            foreach(var obj in _objs) {
                obj.SetActive(true);
            }
            States = new SheepStates(this);
            CurrentState = States.Wander;
            CurrentState.StateEnter();
        }
        else {
            _rb.bodyType = RigidbodyType2D.Kinematic;
            foreach(var col in _col) {
                col.enabled = false;
            }
            foreach(var obj in _objs) {
                obj.SetActive(false);
            }
        }
    }

    [SerializeField] AnimationUI _fallFromSkyAUI;
    [ContextMenu("Play Fall From Sky Animation")]
    public void PlayFallFromSkyAnimation() {
        _fallFromSkyAUI.Play();
        ApplyTrustToWitness();
    }
}
