using System.Collections.Generic;
using UnityEngine;

public class EntityManagerHelperDelete : MonoBehaviour
{
    [SerializeField] EntityManager _entityManager;
    [SerializeField] List<SheepCore> _entitiesToAdd;

    void Awake() {
        _entityManager.HandleGameStateChanged(GameState.Playing);
        _entityManager.Entities.AddRange(_entitiesToAdd);
    }
}
