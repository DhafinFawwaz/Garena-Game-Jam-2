using DhafinFawwaz.ActionExtension;
using UnityEngine;

public class SFXRandomPitcher : MonoBehaviour
{
    [SerializeField] AudioClip[] _audioClips;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] float _minPitch = 0.8f;
    [SerializeField] float _maxPitch = 1.2f;
    [SerializeField] float _volume = 0.4f;
    public void PlayRandomPitchSound() {
        if(_audioClips.Length == 0) return;
        _audioSource.pitch = Random.Range(_minPitch, _maxPitch);
        var clip = _audioClips[Random.Range(0, _audioClips.Length)];
        _audioSource.PlayOneShot(clip, _volume);
    }

    public StaticAction OnStaticEventTriggered;

    void OnEnable() {
        OnStaticEventTriggered += PlayRandomPitchSound;
    }
    void OnDisable() {
        OnStaticEventTriggered -= PlayRandomPitchSound;
    }
}
