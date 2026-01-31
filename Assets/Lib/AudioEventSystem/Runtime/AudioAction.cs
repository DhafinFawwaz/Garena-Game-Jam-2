using System;
using System.Collections.Generic;
using UnityEngine;

namespace DhafinFawwaz.AudioEventSystem
{
public class AudioAction : MonoBehaviour
{
    public List<AudioClipEvent> ClipEvents = new () {new AudioClipEvent(){ Volume=1 }};
    public Action<AudioClip, float> OnAudioNotified;

    void OnEnable() {
        foreach (var clip in ClipEvents) {
            clip.Action += () => {
                OnAudioNotified?.Invoke(clip.Clip, clip.Volume);
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
