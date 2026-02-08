using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [SerializeField] List<SheepCore> _entities = new List<SheepCore>();
    [SerializeField] [ReadOnly] List<Herd> _herds = new List<Herd>();
    [SerializeField] [ReadOnly] bool _isActive;

    public List<SheepCore> Entities => _entities;

    void OnEnable() {
        GameManager.S_OnGameStateChanged += HandleGameStateChanged;
        HerdSpawner.S_OnHerdSpawned += HandleHerdSpawned;
        NeutralSpawner.S_OnNeutralSpawned += HandleNeutralSpawned;
    }

    void OnDisable() {
        GameManager.S_OnGameStateChanged -= HandleGameStateChanged;
        HerdSpawner.S_OnHerdSpawned -= HandleHerdSpawned;
        NeutralSpawner.S_OnNeutralSpawned -= HandleNeutralSpawned;
    }

    public void HandleGameStateChanged(GameState state) {
        _isActive = state == GameState.Playing;
    }

    void HandleHerdSpawned(Herd herd) {
        _herds.Add(herd);
        herd.OnHerdEmpty += HandleHerdEmpty;
        foreach(var member in herd.Members) {
            RegisterEntity(member);
        }
    }

    void HandleNeutralSpawned(SheepCore entity) {
        RegisterEntity(entity);
    }

    void RegisterEntity(SheepCore entity) {
        _entities.Add(entity);
        entity.OnDeath += HandleEntityDeath;
    }

    void Update() {
        if(!_isActive) return;
        foreach(var herd in _herds) {
            herd.DoUpdate();
        }
        foreach(var entity in _entities) {
            entity.DoUpdate();
        }
    }

    void FixedUpdate() {
        if(!_isActive) return;
        foreach(var entity in _entities) {
            entity.DoFixedUpdate();
        }
    }

    [SerializeField] float _trustDecreaseOnDeath = 1f;
    void HandleEntityDeath(SheepCore entity) {
        _entities.Remove(entity);
        // Decrease trust of all other friendly sheep
        foreach(var otherEntity in _entities) {
            if(otherEntity.Stats.TeamID == entity.Stats.TeamID) {
                otherEntity.DecreaseTrust(_trustDecreaseOnDeath);
            }
        }
    }

    void HandleHerdEmpty(Herd herd) {
        _herds.Remove(herd);
    }
}
