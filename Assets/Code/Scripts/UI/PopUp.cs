using DhafinFawwaz.AnimationUI;
using TMPro;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    [SerializeField] AnimationUI _showAnimation;
    [SerializeField] AnimationUI _hideAnimation;

    public void Show() {
        _hideAnimation.Stop();
        _showAnimation.Stop();
        _showAnimation.SetAllTweenTargetValueAsFrom();
        _showAnimation.Play();
    }

    public void Hide() {
        _showAnimation.Stop();
        _hideAnimation.Stop();
        _hideAnimation.SetAllTweenTargetValueAsFrom();
        _hideAnimation.Play();
    }

    [SerializeField] TMP_Text _titleText;
    [SerializeField] TMP_Text _messageText;

    public void SetData(string title, string message) {
        _titleText.text = title;
        _messageText.text = message;
    }
}
