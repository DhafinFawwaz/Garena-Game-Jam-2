using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MouseDownMobileScalingButton : MonoBehaviour
{
    [SerializeField] Transform _targetScale;
    [SerializeField] float _normalScale = 1f;
    [SerializeField] float _downScale = 1.2f;
    [SerializeField] float _duration = 0.2f;

    void Reset() {
        _targetScale = transform;
    }

    public UnityEvent OnClick;

    bool _isMouseDown = false;
    bool _interactable = true;
    void OnMouseDown() {
        if(!_interactable) return;
        if(_isMouseDown) return; 
        _isMouseDown = true;
        StartCoroutine(TweenScale(Vector3.one*_downScale, _duration));
    }

    void OnMouseUp() {
        if(!_isMouseDown) return;
        _isMouseDown = false;
        StartCoroutine(TweenScale(Vector3.one*_normalScale, _duration));
    }


    void OnMouseUpAsButton() {
        if(!_interactable) return;
        OnClick?.Invoke();
    }

    public void SetInteractable(bool interactable) {
        _interactable = interactable;
    }


    byte _key;
    IEnumerator TweenScale(Vector3 targetScale, float duration) {
        byte requirement = ++_key;
        Vector3 originalScale = _targetScale.localScale;
        float elapsedTime = 0f;
        while (elapsedTime < duration && _key == requirement) {
            elapsedTime += Time.deltaTime;
            float t = EaseOutQuart(elapsedTime / duration);
            _targetScale.localScale = Vector3.Lerp(originalScale, targetScale, t);
            yield return null;
        }
        if (_key == requirement) {
            _targetScale.localScale = targetScale;
        }
    }

    float EaseOutQuart(float x) => 1- (1 - x) * (1 - x) * (1 - x) * (1 - x);


    [ContextMenu("Set Scales To Current")]
    public void SetScalesToCurrent() {
        _normalScale = _targetScale.localScale.x;
        _downScale = _normalScale * 1.2f;
    }
}
