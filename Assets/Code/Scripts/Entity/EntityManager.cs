using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [SerializeField] EntitySpawner _entitySpawner;
    [SerializeField] List<Entity> _entities = new List<Entity>();
    void Awake() {
        var newEntities = _entitySpawner.SpawnEntity(Vector2.zero, 10, 5f);
        _entities.AddRange(newEntities);
        foreach(var entity in _entities) {
            entity.OnDeath += HandleEntityDeath;
        }
    }

    void FixedUpdate() {
        foreach(var entity in _entities) {
            entity.DoFixedUpdate();
        }
    }

    void HandleEntityDeath(Entity entity) {
        _entities.Remove(entity);
    }
}
