using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SignDragable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] Sign _signPrefab;
    [SerializeField] Canvas _canvas;
    [SerializeField] CanvasGroup _canvasGroup;
    
    [SerializeField] RectTransform _rt;
    Vector2 _originalPosition;
    Transform _originalParent;
    RectTransform _notDropableArea;
    
    public void Init(Canvas canvas, RectTransform notDropableArea)
    {
        _canvas = canvas;
        _notDropableArea = notDropableArea;
    }
    
    bool _isDragging = false;
    public bool IsDragging => _isDragging;
    public void OnBeginDrag(PointerEventData e)
    {
        _isDragging = true;
        _originalPosition = _rt.anchoredPosition;
        _originalParent = transform.parent;
        
        _canvasGroup.alpha = 0.6f;
        _canvasGroup.blocksRaycasts = false;
        
        transform.SetParent(_canvas.transform);
    }
    public void OnDrag(PointerEventData e)
    {
        _rt.anchoredPosition += e.delta / _canvas.scaleFactor;
    }
    
    public void OnEndDrag(PointerEventData e)
    {
        _isDragging = false;
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
        
        if (!IsInNotDropableArea()) {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(e.position);
            worldPosition.z = 0;
            
            if (_signPrefab != null) {
                Instantiate(_signPrefab, worldPosition, Quaternion.identity);
            }
            
            Destroy(gameObject);
        }
        else {
            transform.SetParent(_originalParent);
            _rt.anchoredPosition = _originalPosition;
        }
    }
    
    bool IsInNotDropableArea() {
        return RectTransformUtility.RectangleContainsScreenPoint(
            _notDropableArea, 
            Mouse.current.position.ReadValue(), 
            _canvas.worldCamera
        );
    }
}