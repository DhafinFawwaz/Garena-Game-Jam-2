using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConveyorBelt : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] List<SignDragable> _signDragablePrefabs = new();
    
    [Header("Settings")]
    [SerializeField] float _speed = 2f;
    [SerializeField] float _spawnInterval = 2f;
    
    [Header("References")]
    [SerializeField] RectTransform _deletePosRt; // if the EntityDragable is at the left of this, destroy
    [SerializeField] RectTransform _spawnPointRt;
    [SerializeField] RectTransform _notDropableArea;
    [SerializeField] Canvas _canvas;
    [SerializeField] RawImage _conveyorBeltImage;
    
    List<SignDragable> _activeSignDragables = new();
    float _spawnTimer;

    void Update() {
        MoveConveyorBelt();
        HandleSpawning();
        DestroyOutOfBounds();
    }
    
    [SerializeField] float _conveyorSpeedMultiplier = 1f;
    void MoveConveyorBelt() {
        for (int i = _activeSignDragables.Count - 1; i >= 0; i--) {
            if (_activeSignDragables[i] != null) {
                if(_activeSignDragables[i].IsDragging) continue;
                RectTransform rt = _activeSignDragables[i].GetComponent<RectTransform>();
                rt.anchoredPosition += Vector2.left * _speed * Time.deltaTime * 100f;
            }
            else {
                _activeSignDragables.RemoveAt(i);
            }
        }

        _conveyorBeltImage.uvRect = new Rect(_conveyorBeltImage.uvRect.x + (_conveyorSpeedMultiplier * Time.deltaTime), 0f, 1f, 1f);
    }
    
    void HandleSpawning() {
        if (_signDragablePrefabs.Count == 0 || _spawnPointRt == null) return;
            
        _spawnTimer += Time.deltaTime;
        
        if (_spawnTimer >= _spawnInterval) {
            _spawnTimer = 0f;
            SpawnRandomSignDragable();
        }
    }
    
    void SpawnRandomSignDragable()
    {
        int randomIndex = Random.Range(0, _signDragablePrefabs.Count);
        SignDragable prefab = _signDragablePrefabs[randomIndex];
        
        SignDragable spawnedObj = Instantiate(prefab, transform);
        RectTransform rt = spawnedObj.GetComponent<RectTransform>();
        rt.anchoredPosition = _spawnPointRt.anchoredPosition;
        
        if (spawnedObj.TryGetComponent<SignDragable>(out SignDragable signDragable)) {
            _activeSignDragables.Add(signDragable);
            signDragable.Init(_canvas, _notDropableArea);
        }
    }
    
    void DestroyOutOfBounds() {
        if (_deletePosRt == null) return;
            
        for (int i = _activeSignDragables.Count - 1; i >= 0; i--) {
            if (_activeSignDragables[i] != null) {
                RectTransform rt = _activeSignDragables[i].GetComponent<RectTransform>();
                
                if (rt.anchoredPosition.x < _deletePosRt.anchoredPosition.x) {
                    Destroy(_activeSignDragables[i].gameObject);
                    _activeSignDragables.RemoveAt(i);
                }
            }
            else {
                _activeSignDragables.RemoveAt(i);
            }
        }
    }
}
