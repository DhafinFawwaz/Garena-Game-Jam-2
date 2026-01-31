using System.Collections.Generic;
using UnityEngine;
namespace DhafinFawwaz.AudioEventSystem
{
public class AudioEventSystemPlayOneShot : MonoBehaviour
{
    public AudioSource AudioSource { get => _audioSource; set => _audioSource = value; }
    [SerializeField] AudioSource _audioSource;
    public List<AudioClipEvent> ClipEvents = new ();

    void OnEnable() {
        foreach (var clip in ClipEvents) {
            clip.Action += () => {
                _audioSource.PlayOneShot(clip.Clip, clip.Volume);
            };
        }
    }

    void OnDisable() {
        foreach (var clip in ClipEvents) {
            clip.Action.RemoveAllListener();
        }
    }
}

}