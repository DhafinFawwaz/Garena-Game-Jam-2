using System;
using DhafinFawwaz.AnimationUI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class JustDragable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] AnimationUI _positionTweener;
    bool _isMouseDown = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        _isMouseDown = true;
        _positionTweener.Stop();
        _spriteRenderer.sortingOrder = 1000;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!_dragable) return;
        if (!_isMouseDown) return;
        _isMouseDown = false;
        _spriteRenderer.sortingOrder = 0;
        MoveToOriginalPosition();
    }
    

    
    /// <summary>
    /// Returns the position of the mouse input or touch input.
    /// </summary>
    /// <returns></returns>
    Vector2 GetInputPosition() {
        if (Touch.activeTouches.Count > 0) {
            return Touch.activeTouches[0].screenPosition;
        } else if (_isMouseDown) {
            return Mouse.current.position.ReadValue();
        }
        return Vector2.zero;
    }

    Camera _mainCam;
    Vector2 _originalPosition;
    void Awake() {
        _mainCam = Camera.main;
        _originalPosition = transform.position;
    }

    
    public Action OnPlaceCorrect;
    void MoveToOriginalPosition() {
        _positionTweener.Stop();
        _positionTweener.Get<PositionTween>(0).SetFrom(transform.position).SetTo(_originalPosition);
        _positionTweener.Play();
    }

    bool _dragable = true;

    void Update() {
        if (_isMouseDown) {
            Vector2 inputPosition = GetInputPosition();
            Vector3 newPos = _mainCam.ScreenToWorldPoint(inputPosition);
            newPos.z = -0.1f; // so that it's in front of other letters
            transform.position = newPos;
        }

    }
}
