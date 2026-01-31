using System.Collections;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TextAppearAnimation : MonoBehaviour
{
    [SerializeField] TMP_Text _dialogText;
    public void ClearText() {
        _dialogText.text = string.Empty;
    }
    [SerializeField] float _delayEachCharacter = 0.05f;
    byte _key = 0;
    [SerializeField] bool _playOnAwake = false;
    [SerializeField] float _playTextDelay = 0;
    public Action OnTextStarted;
    IEnumerator TextAnimation()
    {
        _dialogText.maxVisibleCharacters = 0;
        byte requirement = ++_key;
        if(_playTextDelay > 0) yield return new WaitForSeconds(_playTextDelay);
        float initialTime = Time.time;  
        while(_dialogText.maxVisibleCharacters <= _dialogText.text.Length && _key == requirement) {
            _dialogText.maxVisibleCharacters = (int)((Time.time-initialTime)/_delayEachCharacter);
            yield return null;
        }
    }

    [SerializeField] AudioSource _audioSource;


    [SerializeField] float _playOnAwakeDelay = 1f;
    void Awake() {
        _dialogText.maxVisibleCharacters = 0;
        if (_playOnAwake) {
            Invoke(nameof(Play), _playOnAwakeDelay);
        }
    }

    public void Play() {
        OnTextStarted?.Invoke();
        StartCoroutine(TextAnimation());
        PlayAudio();
    }
    public void PlayText() {
        StartCoroutine(TextAnimation());
    }
    public void PlayAudio() {
        if(_audioSource.clip == null) return;
        _audioSource.Play();
        StartCoroutine(CallOnAudioEnd(_audioSource.clip.length));
    }

    public void SetText(string text) {
        _dialogText.text = text;
        _dialogText.maxVisibleCharacters = 0;
    }

    public void SetClip(AudioClip clip) {
        _audioSource.clip = clip;
    }



    byte _audioKey = 0;
    [SerializeField] float _audioEndDelay = 0;
    IEnumerator CallOnAudioEnd(float delay) {
        byte requirement = ++_audioKey;
        yield return new WaitForSeconds(delay);
        if(_audioEndDelay > 0) yield return new WaitForSeconds(_audioEndDelay);
        if (_audioKey == requirement) {
            _onAudioEnd?.Invoke();
        }
    }

    [Space]
    [SerializeField] UnityEvent _onAudioEnd;
    public UnityEvent OnAudioEnd => _onAudioEnd;
    public void ClearCallOnAudioEnd() {
        _audioKey++;
    }
    public void SetAwakeDelay(float delay) {
        _playOnAwakeDelay = delay;
    }

    public void SetPlayDelayText(float delay) {
        _playTextDelay = delay;
    }
}
