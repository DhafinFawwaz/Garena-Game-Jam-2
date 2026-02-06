using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, ISignInteractable
{
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] float _moveSpeed = 5f;

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
