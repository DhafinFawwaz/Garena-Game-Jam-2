using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField] Entity _entityPrefab;

    public List<Entity> SpawnEntity(Vector2 position, int count, float spawnRadius) {
        List<Entity> entities = new List<Entity>();
        for(int i = 0; i < count; i++) {
            Vector2 spawnPos = position + Random.insideUnitCircle * spawnRadius;
            Entity entity = Instantiate(_entityPrefab, spawnPos, Quaternion.identity);
            entities.Add(entity);
        }
        return entities;
    }
}
