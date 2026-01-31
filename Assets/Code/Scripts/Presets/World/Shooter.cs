using UnityEngine;
using UnityEngine.InputSystem;

public class Shooter : MonoBehaviour
{
    [SerializeField] Projectile _projectilePrefab;
    [SerializeField] float _projectileSpeed = 10f;
    [SerializeField] float _damage = 18f;
    [SerializeField] float _projectileLifetime = 5f;
    [SerializeField] Transform _shootAnchor;
    [SerializeField] Transform _spawnPoint;
    [SerializeField] ParticleSystem _spawnParticle;

    public void Shoot(Vector3 position, Vector2 direction, float speed)
    {
        _spawnParticle.Play();
       var projectile = Instantiate(_projectilePrefab, position, Quaternion.identity);
       projectile.Launch(direction, speed);
       projectile.Damage = _damage;
       Destroy(projectile.gameObject, _projectileLifetime);
    }
    Camera _cam;
    void Awake() {
        _cam = Camera.main;
    }

    void Update() {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldPos = _cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, _cam.nearClipPlane));
        
        Vector3 direction = (worldPos - transform.position);
        direction.z = 0f;
        direction = direction.normalized;

        if(Mouse.current.leftButton.wasPressedThisFrame){
            Shoot(_spawnPoint.position, direction, _projectileSpeed);
        }

        _shootAnchor.right = direction;
    }
}
