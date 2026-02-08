using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] ParticleSystem _explosionVFX;
    [SerializeField] GameObject _parentToDestroy;
    [SerializeField] float _radius = 3f;
    [SerializeField] Transform _explodePoint;
    [SerializeField] Sign _sign;

    void Start()
    {
        if (_sign != null)
            _sign.OnLifetimeEnd += Explode;
    }

    void OnDestroy()
    {
        if (_sign != null)
            _sign.OnLifetimeEnd -= Explode;
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(!col.attachedRigidbody) return;
        if(!col.attachedRigidbody.TryGetComponent<SheepCore>(out var sheepCore)) return;

        Explode();
    }

    void Explode()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(_explodePoint.position, _radius);
        foreach(var hitCollider in hitColliders) {
            if(!hitCollider.attachedRigidbody) continue;
            if(!hitCollider.attachedRigidbody.TryGetComponent<SheepCore>(out var sheep)) continue;
            sheep.OnHurt(new HitRequest{
                Damage=10000,
                Direction=(sheep.transform.position - transform.position).normalized,
                Element=Element.Bomb,
            });
        }
        _explosionVFX.transform.parent = null;
        _explosionVFX.Play();
        Destroy(_parentToDestroy);
        Destroy(_explosionVFX.gameObject, 5f);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
