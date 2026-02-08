using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{
    public SignType Type = SignType.DontGoHere;
    [SerializeField] Sprite _icon;
    public Sprite Icon => _icon;

    float _lifeTime = 10f;
    [SerializeField] float _maxLifetime = 10f;

    void Awake() {
        _lifeTime = _maxLifetime;
    }

    [SerializeField] LifetimePreviewer _lifetimePreviewer;

    List<ISignInteractable> _sheepCores = new ();
    void OnTriggerEnter2D(Collider2D other) {
        if(other.attachedRigidbody == null) return;
        if(!other.attachedRigidbody.TryGetComponent<ISignInteractable>(out var entity)) return;
        entity.OnSignEnter(this);
        _sheepCores.Add(entity);
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.attachedRigidbody == null) return;
        if(!other.attachedRigidbody.TryGetComponent<ISignInteractable>(out var entity)) return;
        entity.OnSignExit(this);
        _sheepCores.Remove(entity);
    }

    void Update() {
        _lifeTime -= Time.deltaTime;
        if(_lifeTime <= 0f) {
            Destroy(gameObject);
            return;
        }
        _lifetimePreviewer.SetValueNormalized(_lifeTime / _maxLifetime);
    }
}
