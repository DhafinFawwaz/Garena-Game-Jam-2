using UnityEngine;

public class Recycle : MonoBehaviour
{
    [SerializeField] ParticleSystem _recycleVFX;
    [SerializeField] GameObject _parentToDestroy;
    [SerializeField] float _radius = 3f;
    [SerializeField] Transform _effectPoint;

    bool _triggered = false;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (_triggered) return;
        if (!col.attachedRigidbody) return;
        if (!col.attachedRigidbody.TryGetComponent<SheepCore>(out var sheepCore)) return;
        if (sheepCore.Herd == null) return;

        _triggered = true;

        Herd herd = sheepCore.Herd;
        herd.Recycle();

        if (_recycleVFX != null)
        {
            _recycleVFX.transform.parent = null;
            _recycleVFX.Play();
            Destroy(_recycleVFX.gameObject, 5f);
        }

        Destroy(_parentToDestroy);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
