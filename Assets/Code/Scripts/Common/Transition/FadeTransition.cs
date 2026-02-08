using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeTransition : SceneTransition
{
    [SerializeField] float _fadeDuration = 0.3f;
    [SerializeField] Color _startColor = new Color(0, 0, 0, 0);
    [SerializeField] Color _endColor = new Color(0, 0, 0, 1);
    [SerializeField] Image _fadeImage;

    protected override IEnumerator TransitionOutAnimation() {
        _fadeImage.color = _startColor;
        yield return FadeToColor(_endColor, _fadeDuration);
    }

    protected override IEnumerator TransitionInAnimation() {
        Debug.Log("Transition In Animation");
        yield return FadeToColor(_startColor, _fadeDuration);
    }

    byte _key;
    IEnumerator FadeToColor(Color targetColor, float duration){
        byte requirement = ++_key;
        float elapsedTime = 0f;
        Color initialColor = _fadeImage.color;
        while (elapsedTime < duration && _key == requirement) {
            elapsedTime += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            _fadeImage.color = Color.Lerp(initialColor, targetColor, EaseOutQuartic(t));
            yield return null;
        }
        if (_key == requirement) {
            _fadeImage.color = targetColor;
        }
    }

    float EaseOutQuartic(float t) => 1 - (1 - t) * (1 - t) * (1 - t) * (1 - t);
}
