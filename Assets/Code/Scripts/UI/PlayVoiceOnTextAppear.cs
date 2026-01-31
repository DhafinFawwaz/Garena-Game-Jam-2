using UnityEngine;

public class PlayVoiceOnTextAppear : MonoBehaviour
{
    [SerializeField] AudioClip _voiceClip;
    [SerializeField] TextAppearAnimation _textAppearAnimation;
    [SerializeField] AudioManager _audioManager;
    void Reset() {
        _textAppearAnimation = GetComponent<TextAppearAnimation>();
        _audioManager = GetComponent<AudioManager>();
    }

    void OnEnable() {
        _textAppearAnimation.OnTextStarted += PlayVoice;
    }
    void OnDisable() {
        _textAppearAnimation.OnTextStarted -= PlayVoice;
    }

    public void PlayVoice() {
        _textAppearAnimation.SetClip(_voiceClip);
        _textAppearAnimation.PlayAudio();
    }
}