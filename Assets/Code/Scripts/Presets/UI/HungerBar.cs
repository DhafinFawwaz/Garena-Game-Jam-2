using DhafinFawwaz.AnimationUI;
using UnityEngine;

public class HungerBar : MonoBehaviour
{
    [SerializeField] AnimationUI _colorAUI;
    [SerializeField] AnimationUI _whiteAUI;

    public void SetHungerNormalized(float newNormalized)
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

    public void SetHungerNormalizedImmediate(float normalized)
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
    [ContextMenu("Set Hunger to 100%")]
    public void SetHungerTo100() => SetHungerNormalized(1.0f);
    [ContextMenu("Set Hunger to 80%")]
    public void SetHungerTo80() => SetHungerNormalized(0.8f);
    [ContextMenu("Set Hunger to 50%")]
    public void SetHungerTo50() => SetHungerNormalized(0.5f);
    [ContextMenu("Set Hunger to 20%")]
    public void SetHungerTo20() => SetHungerNormalized(0.2f);
    [ContextMenu("Set Hunger to 0%")]
    public void SetHungerTo0() => SetHungerNormalized(0.0f);
#endif
}
