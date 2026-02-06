using UnityEngine;

public class LifetimePreviewer : MonoBehaviour
{
    [SerializeField] Transform _fillTransform;
    public void SetValueNormalized(float normalizedValue) {
        Vector3 localScale = _fillTransform.localScale;
        localScale.x = Mathf.Clamp01(normalizedValue);
        _fillTransform.localScale = localScale;
    }
}