using System;
using DhafinFawwaz.AnimationUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPopup : MonoBehaviour
{
    [SerializeField] AnimationUI _showAnimation;
    [SerializeField] AnimationUI _hideAnimation;
    [SerializeField] TMP_Text _titleText;
    [SerializeField] TMP_Text _messageText;
    [SerializeField] Image _image;
    [SerializeField] GameObject _imageContainer;
    [SerializeField] Button _nextButton;
    [SerializeField] TMP_Text _nextButtonText;

    public event Action OnNextClicked;

    void Awake()
    {
        _nextButton.onClick.AddListener(() => OnNextClicked?.Invoke());
    }

    public void Show(TutorialStep step, bool isLastStep)
    {
        _titleText.text = step.Title;
        _messageText.text = step.Message;

        if (_imageContainer != null)
        {
            bool hasImage = step.Image != null;
            _imageContainer.SetActive(hasImage);
            if (hasImage) _image.sprite = step.Image;
        }

        if (_nextButtonText != null)
            _nextButtonText.text = isLastStep ? "Got it!" : "Next";

        _hideAnimation.Stop();
        _showAnimation.Stop();
        _showAnimation.SetAllTweenTargetValueAsFrom();
        _showAnimation.Play();
    }

    public void Hide()
    {
        _showAnimation.Stop();
        _hideAnimation.Stop();
        _hideAnimation.SetAllTweenTargetValueAsFrom();
        _hideAnimation.Play();
    }
}
