using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConveyorBelt : MonoBehaviour
{
    public static ConveyorBelt Instance { get; private set; }

    [Header("Prefabs")]
    [SerializeField] SignDragable _signDragableTemplate;
    [SerializeField] List<Sign> _signPrefabs = new();

    [Header("Settings")]
    [SerializeField] float _speed = 2f;
    [SerializeField] float _spawnInterval = 2f;
    [SerializeField] int _maxActiveSigns = 5;
    float _spawnIntervalMultiplier = 1f;
    
    [Header("References")]
    [SerializeField] RectTransform _deletePosRt; // if the EntityDragable is at the left of this, destroy
    [SerializeField] RectTransform _spawnPointRt;
    [SerializeField] RectTransform _notDropableArea;
    [SerializeField] Canvas _canvas;
    [SerializeField] RawImage _conveyorBeltImage;
    
    List<SignDragable> _activeSignDragables = new();
    float _spawnTimer;
    bool _isActive;

    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        GameManager.S_OnGameStateChanged += HandleGameStateChanged;
    }

    void OnDisable()
    {
        GameManager.S_OnGameStateChanged -= HandleGameStateChanged;
    }

    void HandleGameStateChanged(GameState state)
    {
        _isActive = state == GameState.Playing;
    }

    void Update() {
        if (!_isActive) return;
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
        if (_signPrefabs.Count == 0 || _spawnPointRt == null || _signDragableTemplate == null) return;
        if (_activeSignDragables.Count >= _maxActiveSigns) return;

        _spawnTimer += Time.deltaTime;

        if (_spawnTimer >= _spawnInterval * _spawnIntervalMultiplier) {
            _spawnTimer = 0f;
            SpawnRandomSignDragable();
        }
    }

    void SpawnRandomSignDragable()
    {
        int randomIndex = Random.Range(0, _signPrefabs.Count);
        Sign signPrefab = _signPrefabs[randomIndex];

        SignDragable spawnedObj = Instantiate(_signDragableTemplate, transform);
        RectTransform rt = spawnedObj.GetComponent<RectTransform>();
        rt.anchoredPosition = _spawnPointRt.anchoredPosition;

        _activeSignDragables.Add(spawnedObj);
        spawnedObj.Init(_canvas, _notDropableArea, signPrefab);
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

    public void AddSignPrefab(Sign prefab)
    {
        if (!_signPrefabs.Contains(prefab))
            _signPrefabs.Add(prefab);
    }

    public void SetSpawnInterval(float interval)
    {
        _spawnInterval = interval;
    }

    public void SetMaxActiveSigns(int max)
    {
        _maxActiveSigns = max;
    }

    public float GetSpawnInterval()
    {
        return _spawnInterval;
    }

    public void SetSpawnIntervalMultiplier(float multiplier)
    {
        _spawnIntervalMultiplier = multiplier;
    }
}
