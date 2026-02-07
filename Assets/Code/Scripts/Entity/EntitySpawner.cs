using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField] SheepCore _entityPrefab;

    public List<SheepCore> SpawnEntity(Vector2 position, int count, float spawnRadius) {
        List<SheepCore> entities = new List<SheepCore>();
        for(int i = 0; i < count; i++) {
            Vector2 spawnPos = position + Random.insideUnitCircle * spawnRadius;
            SheepCore entity = Instantiate(_entityPrefab, spawnPos, Quaternion.identity);
            entity.Stats.CurrentTrust = Random.Range(0f, 100f);
            entities.Add(entity);
        }
        return entities;
    }
}
