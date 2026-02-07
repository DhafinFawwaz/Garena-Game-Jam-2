using DhafinFawwaz.AnimationUI;
using UnityEngine;

public class EntityCliff : MonoBehaviour
{
    [SerializeField] Collider2D _cliffCollider;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] AnimationUI _fallAUI;
    [SerializeField] SheepCore _sheepCore;
    void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.CompareTag("Cliff")) {
            _cliffCollider.enabled = false;
            _rb.bodyType = RigidbodyType2D.Static;
            _fallAUI.Play();

            _sheepCore.SetActiveCore(false);
            _sheepCore.SwitchState(_sheepCore.States.Death);
            _sheepCore.OnDeath?.Invoke(_sheepCore);
            Destroy(gameObject, 5f);
        }
    }
}
