using UnityEngine;
using TMPro;
public class TextBlinkingAnimation : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
    [SerializeField] float _period = 0.5f;

    void Reset()
    {
        _text = GetComponent<TMP_Text>();
        if (_text == null)
            _text = GetComponentInChildren<TMP_Text>();
        if (_text == null)
            _text = GetComponentInParent<TMP_Text>();
    }
    void Update() => _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, SineWave(Time.time, _period));

    float SineWave(float x, float period) => Mathf.Sin(2*Mathf.PI*x/period) * 0.5f + 0.5f;
}
