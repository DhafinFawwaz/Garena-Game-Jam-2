using System.Collections;
using UnityEngine;

public class SubtitleOrderer : MonoBehaviour
{
    [SerializeField] Subtitle[] _subtitles;
    [SerializeField] bool _playOnAwake = true;
    [SerializeField] float _playOnAwakeDelay = 0.75f;
    void Awake() {
        if (_playOnAwake) Invoke(nameof(Play), _playOnAwakeDelay);
    }
    
    public void Play() {
        PlayRecursive(0);
    }

    void PlayRecursive(int idx) {
        if (idx >= _subtitles.Length) return;
        _subtitles[idx].Appear();
        if (idx + 1 < _subtitles.Length) {
            Debug.Log("Set on text end for " + idx);
            _subtitles[idx].SetOnTextEnd(() => {
                _subtitles[idx].Disappear();
                PlayRecursive(idx + 1);
            });
        }
    }
}
