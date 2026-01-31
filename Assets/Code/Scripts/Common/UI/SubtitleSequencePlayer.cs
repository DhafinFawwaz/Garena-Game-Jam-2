using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SubtitleSequencePlayer : MonoBehaviour {
    public bool PlayOnAwake = false;
    [SerializeField] TextAppearAnimation _textAppearAnimation;
    public SubtitleData[] subtitles;

    [SerializeField] float _delayEachSubtitle = 0.1f;


    void Awake() {  
        if (PlayOnAwake) {
            Play();
        }
    }

    public void Play() {
        if (_coroutine != null) {
            StopCoroutine(_coroutine);
        }
        _coroutine = StartCoroutine(PlaySequence());
    }

    Coroutine _coroutine;

    IEnumerator PlaySequence() {
        _textAppearAnimation.ClearText();
        int idx = 0;
        foreach (var subtitle in subtitles) {
            if (subtitle.IsDelaySameAsPreviousClipDuration && _textAppearAnimation != null && idx > 0) {
                var previousClip = subtitles[idx - 1].AudioClip;
                if (previousClip != null) {
                    subtitle.Delay = previousClip.length;
                }
            }
            yield return new WaitForSeconds(subtitle.Delay);
            _textAppearAnimation.SetClip(subtitle.AudioClip);
            _textAppearAnimation.PlayAudio();
            yield return new WaitForSeconds(subtitle.DelayTextAfterClipStart);
            _textAppearAnimation.SetText(subtitle.Text);
            _textAppearAnimation.PlayText();

            yield return new WaitForSeconds(_delayEachSubtitle);
            idx++;
        }
        _onSequenceEnd?.Invoke();
    }
    [SerializeField] UnityEvent _onSequenceEnd;
    public void ClearText(float delay = 0) {
        StartCoroutine(CallOnAudioEnd(delay));
    }
    IEnumerator CallOnAudioEnd(float duration) {
        yield return new WaitForSeconds(duration);
        _textAppearAnimation.ClearText();
    }
}

[System.Serializable]
public class SubtitleData {
    public bool IsDelaySameAsPreviousClipDuration = true;
    public float Delay;
    public AudioClip AudioClip;
    public float DelayTextAfterClipStart;
    [TextArea]
    public string Text;
}