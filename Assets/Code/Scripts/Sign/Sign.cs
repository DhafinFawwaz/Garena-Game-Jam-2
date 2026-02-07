using UnityEngine;

public class Sign : MonoBehaviour
{
    [SerializeField] float _lifeTime = 10f;
    [SerializeField] LifetimePreviewer _lifetimePreviewer;
    void OnTriggerEnter2D(Collider2D other) {
        if(other.attachedRigidbody == null) return;
        Debug.Log("Trigger entered by " + other.attachedRigidbody.name);
        if(!other.attachedRigidbody.TryGetComponent<ISignInteractable>(out var entity)) return;
        Debug.Log("Sign entered by " + other.attachedRigidbody.name);
        entity.OnSignEnter(this);
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.attachedRigidbody == null) return;
        if(!other.attachedRigidbody.TryGetComponent<ISignInteractable>(out var entity)) return;
        entity.OnSignExit(this);
    }

    void Update() {
        _lifeTime -= Time.deltaTime;
        if(_lifeTime <= 0f) {
            Destroy(gameObject);
            return;
        }
        _lifetimePreviewer.SetValueNormalized(_lifeTime / 10f);
    }
}
