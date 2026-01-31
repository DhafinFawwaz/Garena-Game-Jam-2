using DhafinFawwaz.AnimationUI;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    [SerializeField] AnimationUI _colorAUI;
    [SerializeField] AnimationUI _whiteAUI;

    public void SetHealthNormalized(float newNormalized)
    {
        _colorAUI.Stop();
        _colorAUI.Get<LocalScaleTween>(0)
            .SetTo(new Vector3(newNormalized, 1f, 1f))
            .SetFromAsTargetValueSafe();
        _colorAUI.Play();

        _whiteAUI.Stop();
        _whiteAUI.Get<LocalScaleTween>(0)
            .SetTo(new Vector3(newNormalized, 1f, 1f))
            .SetFromAsTargetValueSafe();
        _whiteAUI.Play();
    }

    public void SetHealthNormalizedImmediate(float normalized)
    {
        _colorAUI.Stop();
        _colorAUI.Get<LocalScaleTween>(0)
            .SetTo(new Vector3(normalized, 1f, 1f))
            .SetFromAsTargetValueSafe();
        _colorAUI.ApplyToFinish();

        _whiteAUI.Stop();
        _whiteAUI.Get<LocalScaleTween>(0)
            .SetTo(new Vector3(normalized, 1f, 1f))
            .SetFromAsTargetValueSafe();
        _whiteAUI.ApplyToFinish();
    }

#if UNITY_EDITOR
    [ContextMenu("Set Health to 100%")]
    public void SetHealthTo100() => SetHealthNormalized(1.0f);
    [ContextMenu("Set Health to 80%")]
    public void SetHealthTo80() => SetHealthNormalized(0.8f);
    [ContextMenu("Set Health to 50%")]
    public void SetHealthTo50() => SetHealthNormalized(0.5f);
    [ContextMenu("Set Health to 20%")]
    public void SetHealthTo20() => SetHealthNormalized(0.2f);
    [ContextMenu("Set Health to 0%")]
    public void SetHealthTo0() => SetHealthNormalized(0.0f);

#endif
}
