using UnityEngine;

public class TransformSineScaleOvertime : MonoBehaviour
{
    [SerializeField] float _minScale = 1f;
    [SerializeField] float _maxScale = 1.2f;
    [SerializeField] float _sineSpeed = 2f;
    void Update()
    {
        transform.localScale = Vector3.one * Mathf.LerpUnclamped(_minScale, _maxScale, Sine0to1(Time.time * _sineSpeed));
    }

    float Sine0to1(float time) {
        return (Mathf.Sin(time) + 1f) / 2f + 0.5f;
    }
}
