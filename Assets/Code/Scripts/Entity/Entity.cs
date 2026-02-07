using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, ISignInteractable, ITrustable
{
    public static Action<TrustEvent> S_OnTrustChanged;

    [SerializeField] Rigidbody2D _rb;
    [SerializeField] float _moveSpeed = 5f;

    [SerializeField] [ReadOnly] float _trustLevel = 0.5f;
    public float TrustLevel { get => _trustLevel; }

    Herd _herd;
    public Herd Herd { get => _herd; }

    public void Init(HerdData data) {
        _moveSpeed = data.MoveSpeed;
        _trustLevel = data.InitialTrustLevel;
    }

    public void SetHerd(Herd herd) {
        _herd = herd;
    }

    public void IncreaseTrust(float amount) {
        _trustLevel = Mathf.Clamp01(_trustLevel + amount);
        S_OnTrustChanged?.Invoke(new TrustEvent { Entity = this, TrustLevel = _trustLevel });
    }

    public void DecreaseTrust(float amount) {
        _trustLevel = Mathf.Clamp01(_trustLevel - amount);
        S_OnTrustChanged?.Invoke(new TrustEvent { Entity = this, TrustLevel = _trustLevel });
    }

    public void DoFixedUpdate() {
        if(_currentSigns.Count == 0) return;
        var sign = _currentSigns[0];
        Vector2 direction = (sign.transform.position - transform.position).normalized;
        // _rb.MovePosition(_rb.position + direction * _moveSpeed * Time.fixedDeltaTime);
        _rb.linearVelocity = direction * _moveSpeed;
        Debug.DrawLine(transform.position, sign.transform.position, Color.red);
    }

    [SerializeField] [ReadOnly] List<Sign> _currentSigns = new List<Sign>();
    public void OnSignEnter(Sign sign) {
        if(_trustLevel <= 0f) return;
        _currentSigns.Add(sign);
    }

    public void OnSignExit(Sign sign) {
        _currentSigns.Remove(sign);
        if(_currentSigns.Count == 0) {
            _rb.linearVelocity = Vector2.zero;
        }
    }

    public Action<Entity> OnDeath;
    public void Die() {
        OnDeath?.Invoke(this);
        Destroy(gameObject);
    }
}
