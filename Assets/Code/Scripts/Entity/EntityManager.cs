using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [SerializeField] EntitySpawner _entitySpawner;
    [SerializeField] List<SheepCore> _entities = new ();
    [SerializeField] int _spawnCount = 1;
    void Awake() {
        var newEntities = _entitySpawner.SpawnEntity(Vector2.zero, _spawnCount, 5f);
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
    void Update() {
        foreach(var entity in _entities) {
            entity.DoUpdate();
        }
    }

    void HandleEntityDeath(SheepCore entity) {
        _entities.Remove(entity);
    }
}
