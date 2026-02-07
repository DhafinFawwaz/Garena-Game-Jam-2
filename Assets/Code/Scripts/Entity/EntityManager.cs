using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [SerializeField] [ReadOnly] List<Entity> _entities = new List<Entity>();
    [SerializeField] [ReadOnly] List<Herd> _herds = new List<Herd>();
    [SerializeField] [ReadOnly] bool _isActive;

    void OnEnable() {
        GameManager.S_OnGameStateChanged += HandleGameStateChanged;
        HerdSpawner.S_OnHerdSpawned += HandleHerdSpawned;
    }

    void OnDisable() {
        GameManager.S_OnGameStateChanged -= HandleGameStateChanged;
        HerdSpawner.S_OnHerdSpawned -= HandleHerdSpawned;
    }

    void HandleGameStateChanged(GameState state) {
        _isActive = state == GameState.Playing;
    }

    void HandleHerdSpawned(Herd herd) {
        _herds.Add(herd);
        herd.OnHerdEmpty += HandleHerdEmpty;
        foreach(var member in herd.Members) {
            RegisterEntity(member);
        }
    }

    void RegisterEntity(Entity entity) {
        _entities.Add(entity);
        entity.OnDeath += HandleEntityDeath;
    }

    void Update() {
        if(!_isActive) return;
        foreach(var herd in _herds) {
            herd.DoUpdate();
        }
    }

    void FixedUpdate() {
        if(!_isActive) return;
        foreach(var entity in _entities) {
            entity.DoFixedUpdate();
        }
    }

    void HandleEntityDeath(Entity entity) {
        _entities.Remove(entity);
    }

    void HandleHerdEmpty(Herd herd) {
        _herds.Remove(herd);
    }
}
