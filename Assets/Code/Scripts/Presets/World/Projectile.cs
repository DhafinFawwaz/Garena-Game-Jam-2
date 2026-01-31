using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] float _damage = 10f;
    public float Damage { get => _damage; set => _damage = value; }
    public void Launch(Vector2 direction, float speed){
        _rb.linearVelocity = direction.normalized * speed;
        _rb.transform.right = direction;
    }   

    [SerializeField] ParticleSystem _hitParticle;

    bool GetHurtable(Collider2D col, out IHurtable hurtable){
        if(!col.attachedRigidbody) {
            hurtable = null;
            return false;
        }
        if(!col.attachedRigidbody.TryGetComponent(out hurtable)) return false;
        return true;
    }

    void OnTriggerEnter2D(Collider2D col) {

        Destroy(gameObject);
        _hitParticle.transform.parent = null;
        _hitParticle.Play();
        Destroy(_hitParticle.gameObject, 5f);

        if(!GetHurtable(col, out IHurtable hurtable)) return;
        var res = hurtable.OnHurt(new HitRequest { Damage = _damage });
    }
}
