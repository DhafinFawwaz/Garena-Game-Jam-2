using System.Collections.Generic;
using DhafinFawwaz.AnimationUI;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
public class LetterDragable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public char Letter;
    [SerializeField] HashSet<LetterPlacable> _placables = new ();
    [ReadOnly] [SerializeField] Vector2 _originalPosition;
    [SerializeField] TMP_Text _letterText;
    // void OnValidate() {
    //     _letterText.text = Letter.ToString();
    // }
    public void SetLetter(char letter) {
        Letter = letter;
        _letterText.text = letter.ToString();
        #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.EditorUtility.SetDirty(_letterText);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
        #endif
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(col.TryGetComponent<LetterPlacable>(out var placable)) {
            _placables.Add(placable);
        }
    }
    void OnTriggerExit2D(Collider2D col) {
        if(col.TryGetComponent<LetterPlacable>(out var placable)) {
            _placables.Remove(placable);
            placable.Unhighlight(this);
        }
    }



    bool _isMouseDown = false;
    [SerializeField] UnityEvent _onMouseDown;
    public void OnPointerDown(PointerEventData eventData) {
        _isMouseDown = true;
        _positionTweener.Stop();
        if(_currentPlacable != null) {
            _currentPlacable.Unplace();
            _currentPlacable = null;
        }
        _onMouseDown?.Invoke();
    }


    LetterPlacable _currentPlacable;
    public void OnPointerUp(PointerEventData eventData) {
        _isMouseDown = false;

        if(TryGetClosestPlacable(out LetterPlacable closestPlacable)) {
            if(closestPlacable.PlacedLetter != null && closestPlacable.PlacedLetter != this) {
                closestPlacable.PlacedLetter.MoveToOriginalPosition();
                closestPlacable.PlacedLetter._currentPlacable = null;
                closestPlacable.Unplace();
                closestPlacable.PlacedLetter = null;
            }
            closestPlacable.Place(this);
            _currentPlacable = closestPlacable;
            _positionTweener.Stop();
            _positionTweener.Get<PositionTween>(0).SetFrom(transform.position).SetTo(new Vector2(closestPlacable.transform.position.x, closestPlacable.transform.position.y));
            _positionTweener.Play();

            foreach (var placable in _placables) {
                placable.Unhighlight(this);
            }
        } else {
            MoveToOriginalPosition();
            _currentPlacable = null;
            foreach (var placable in _placables) {
                placable.Unhighlight(this);
            }
        }
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
    void Awake() {
        _mainCam = Camera.main;
        _originalPosition = transform.position;
        // _letterText.text = Letter.ToString();
        StartCoroutine(SetOriginalPositionDelayed());
    }
    IEnumerator SetOriginalPositionDelayed() {
        yield return null;
        yield return null;
        _originalPosition = transform.position;
    }

    [SerializeField] AnimationUI _positionTweener;
    void MoveToOriginalPosition() {
        _positionTweener.Stop();
        _positionTweener.Get<PositionTween>(0).SetFrom(transform.position).SetTo(_originalPosition);
        _positionTweener.Play();
    }

    void Update() {
        if (_isMouseDown) {
            Vector2 inputPosition = GetInputPosition();
            Vector3 newPos = _mainCam.ScreenToWorldPoint(inputPosition);
            newPos.z = -0.1f; // so that it's in front of other letters
            transform.position = newPos;
        }
        if(_isMouseDown && TryGetClosestPlacable(out LetterPlacable closestPlacable)) {
            closestPlacable.Highlight(this);
            foreach (var placable in _placables) {
                if (placable != closestPlacable) {
                    placable.Unhighlight(this);
                }
            }
        } else {
            foreach (var placable in _placables) {
                placable.Unhighlight(this);
            }
        }
    }

    bool TryGetClosestPlacable(out LetterPlacable closestPlacable) {
        closestPlacable = null;
        float closestDistance = float.MaxValue;

        foreach (var placable in _placables) {
            float distance = Vector2.Distance(transform.position, placable.transform.position);
            if (distance < closestDistance) {
                closestDistance = distance;
                closestPlacable = placable;
            }
        }

        return closestPlacable != null;
    }

}
