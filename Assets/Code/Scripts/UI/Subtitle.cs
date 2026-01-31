using System;
using DhafinFawwaz.AnimationUI;
using UnityEngine;

public class Subtitle : MonoBehaviour
{
    [SerializeField] AnimationUI _appear;
    [SerializeField] AnimationUI _disappear;
    [SerializeField] TextAppearAnimation _textAppearAnimation;

    public void SetOnTextEnd(Action action) {
        _textAppearAnimation.OnAudioEnd.AddListener(action.Invoke);
    }

    public void Appear() {
        _appear.Play();
    }

    public void Disappear() {
        _disappear.Play();
    }
}
