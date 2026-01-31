using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace DhafinFawwaz.AudioEventSystem
{
public class AudioUnityEvent : MonoBehaviour
{
    public List<AudioClipEvent> ClipEvents = new ();
    public UnityEvent<AudioClip, float> OnAudioNotified;

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