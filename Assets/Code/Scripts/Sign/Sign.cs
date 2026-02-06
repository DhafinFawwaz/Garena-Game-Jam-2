using UnityEngine;

public class Sign : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) {
        if(other.attachedRigidbody == null) return;
        if(!other.attachedRigidbody.TryGetComponent<ISignInteractable>(out var entity)) return;
        entity.OnSignEnter(this);
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.attachedRigidbody == null) return;
        if(!other.attachedRigidbody.TryGetComponent<ISignInteractable>(out var entity)) return;
        entity.OnSignExit(this);
    }
}
