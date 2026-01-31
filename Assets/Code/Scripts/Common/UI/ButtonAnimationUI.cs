using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Events;
using System;
using System.Linq;
using System.Collections.Generic;
using DhafinFawwaz.AnimationUI;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;



#if UNITY_EDITOR
using UnityEditor.Events;
#endif

public class ButtonAnimationUI : Selectable, IPointerClickHandler, ISubmitHandler
{
    public Graphic TargetGraphic { get => targetGraphic; set => targetGraphic = value; }

    [SerializeField] AnimationUI _pointerClickAnimation;
    [SerializeField] AnimationUI _pointerDownAnimation;
    [SerializeField] AnimationUI _pointerUpAnimation;
    [SerializeField] AnimationUI _pointerEnterAnimation;
    [SerializeField] AnimationUI _pointerExitAnimation;
    [SerializeField] AnimationUI _onSelectAnimation;
    [SerializeField] AnimationUI _onDeselectAnimation;
    [SerializeField] AnimationUI _toInteractableAnimation;
    [SerializeField] AnimationUI _notInteractableAnimation;
    public UnityEvent onClick;
    public UnityEvent onPointerDown;
    public UnityEvent onPointerUp;
    public UnityEvent onPointerEnter;
    public UnityEvent onPointerExit;

    public static Action<ButtonAnimationUI> s_OnClickPassSelf;
    public static Action<ButtonAnimationUI> s_OnPointerDownPassSelf;
    public static Action<ButtonAnimationUI> s_OnPointerUpPassSelf;
    public static Action<ButtonAnimationUI> s_OnPointerEnterPassSelf;
    public static Action<ButtonAnimationUI> s_OnPointerExitPassSelf;
    public static Action<ButtonAnimationUI> s_OnSelectPassSelf;
    public static Action<ButtonAnimationUI> s_OnDeselectPassSelf;

    public static Action s_OnClick;
    public static Action s_OnPointerDown;
    public static Action s_OnPointerUp;
    public static Action s_OnPointerEnter;
    public static Action s_OnPointerExit;
    public static Action s_OnSelect;
    public static Action s_OnDeselect;


    public void StopAllAnimations() {
        _pointerDownAnimation?.Stop();
        _pointerUpAnimation?.Stop();
        _pointerClickAnimation?.Stop();
        _onSelectAnimation?.Stop();
        _onDeselectAnimation?.Stop();
        _notInteractableAnimation?.Stop();
        _toInteractableAnimation?.Stop();
    }
    public void OnPointerClick(PointerEventData eventData) => OnSubmitOrClick();
    public void OnSubmit(BaseEventData eventData) => OnSubmitOrClick();
    void OnSubmitOrClick() {
        if(!interactable) return;
        StopAllAnimations();
        _pointerClickAnimation?.Play();
        onClick?.Invoke();
        s_OnClickPassSelf?.Invoke(this);
        s_OnClick?.Invoke();
    }

    public override void OnPointerDown(PointerEventData eventData) {
        if(!interactable) return;
        StopAllAnimations();
        _pointerDownAnimation?.SetAllTweenTargetValueAsFrom();
        _pointerDownAnimation?.Play();
        onPointerDown?.Invoke();
        s_OnPointerDownPassSelf?.Invoke(this);
        s_OnPointerDown?.Invoke();
    }

    public override void OnPointerUp(PointerEventData eventData) {
        if(!interactable) return;
        StopAllAnimations();
        _pointerUpAnimation?.SetAllTweenTargetValueAsFrom();
        _pointerUpAnimation?.Play();
        onPointerUp?.Invoke();
        s_OnPointerUpPassSelf?.Invoke(this);
        s_OnPointerUp?.Invoke();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if(!interactable) return;
        StopAllAnimations();
        _pointerEnterAnimation?.SetAllTweenTargetValueAsFrom();
        _pointerEnterAnimation?.Play();
        onPointerEnter?.Invoke();
        s_OnPointerEnterPassSelf?.Invoke(this);
        s_OnPointerEnter?.Invoke();
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if(!interactable) return;
        StopAllAnimations();
        _pointerExitAnimation?.SetAllTweenTargetValueAsFrom();
        _pointerExitAnimation?.Play();
        onPointerExit?.Invoke();
        s_OnPointerExitPassSelf?.Invoke(this);
        s_OnPointerExit?.Invoke();
    }

    public override void OnSelect(BaseEventData eventData)
    {
        if(!interactable) return;
        StopAllAnimations();
        _onSelectAnimation?.SetAllTweenTargetValueAsFrom();
        _onSelectAnimation?.Play();
        s_OnSelectPassSelf?.Invoke(this);
        s_OnSelect?.Invoke();
    }
    public override void OnDeselect(BaseEventData eventData)
    {
        if(!interactable) return;
        StopAllAnimations();
        _onDeselectAnimation?.SetAllTweenTargetValueAsFrom();
        _onDeselectAnimation?.Play();
        s_OnDeselectPassSelf?.Invoke(this);
        s_OnDeselect?.Invoke();
    }
    
    public bool Interactable {
        get => interactable;
        set {
            if(interactable == value) return;
            interactable = value;
            if(!interactable) {
                _notInteractableAnimation?.SetAllTweenTargetValueAsFrom();
                _notInteractableAnimation?.Play();
            } else {
                _toInteractableAnimation?.SetAllTweenTargetValueAsFrom();
                _toInteractableAnimation?.Play();
            }
        }
    }
    public void SetInteractable(bool value) {
        Interactable = value;
    }

    protected override void Awake() {
        base.Awake();
        if(!interactable) _notInteractableAnimation?.ApplyToFinish();
    }


#if UNITY_EDITOR
    protected override void Reset() {
        base.Reset();
        transition = Transition.None;
        InitAnimationUIs();

        float duration = 0.15f;
        var normalScale = 1f * Vector2.one;
        var bigScale = 1.1f * Vector2.one;
        var biggerScale = 1.2f * Vector2.one;
        var ease = DhafinFawwaz.AnimationUI.Ease.OutBackQuart;

        Image img = transform.GetChild(0).GetComponent<Image>();
        _pointerClickAnimation.     Add(new LocalScaleTween{Target=img.transform, Duration=duration, EaseType=ease, From=biggerScale, To=normalScale});
        _pointerDownAnimation.      Add(new LocalScaleTween{Target=img.transform, Duration=duration, EaseType=ease, From=normalScale, To=biggerScale});
        _pointerUpAnimation.        Add(new LocalScaleTween{Target=img.transform, Duration=duration, EaseType=ease, From=bigScale, To=normalScale});
        _pointerEnterAnimation.     Add(new LocalScaleTween{Target=img.transform, Duration=duration, EaseType=ease, From=normalScale, To=bigScale});
        _pointerExitAnimation.      Add(new LocalScaleTween{Target=img.transform, Duration=duration, EaseType=ease, From=bigScale, To=normalScale});
        _onSelectAnimation.         Add(new LocalScaleTween{Target=img.transform, Duration=duration, EaseType=ease, From=normalScale, To=bigScale});
        _onDeselectAnimation.       Add(new LocalScaleTween{Target=img.transform, Duration=duration, EaseType=ease, From=bigScale, To=normalScale});
        _toInteractableAnimation.   Add(new LocalScaleTween{Target=img.transform, Duration=duration, EaseType=ease, From=normalScale, To=biggerScale});
        _notInteractableAnimation.  Add(new LocalScaleTween{Target=img.transform, Duration=duration, EaseType=ease, From=normalScale, To=biggerScale});
    }

    void InitAnimationUIs() {
        CreateAnimationUIChildIfNotExist(ref _pointerClickAnimation, "[AnimationUI] OnPointerClick");
        CreateAnimationUIChildIfNotExist(ref _pointerDownAnimation, "[AnimationUI] OnPointerDown");
        CreateAnimationUIChildIfNotExist(ref _pointerUpAnimation, "[AnimationUI] OnPointerUp");
        CreateAnimationUIChildIfNotExist(ref _pointerEnterAnimation, "[AnimationUI] OnPointerEnter");
        CreateAnimationUIChildIfNotExist(ref _pointerExitAnimation, "[AnimationUI] OnPointerExit");
        CreateAnimationUIChildIfNotExist(ref _onSelectAnimation, "[AnimationUI] OnSelect");
        CreateAnimationUIChildIfNotExist(ref _onDeselectAnimation, "[AnimationUI] OnDeselect");
        CreateAnimationUIChildIfNotExist(ref _toInteractableAnimation, "[AnimationUI] ToInteractable");
        CreateAnimationUIChildIfNotExist(ref _notInteractableAnimation, "[AnimationUI] NotInteractable");
    }

    void CreateAnimationUIChildIfNotExist(ref AnimationUI animationUI, string name) {
        if(animationUI != null) return;
        GameObject child = new GameObject(name);
        child.transform.SetParent(transform.GetChild(1), false);
        child.transform.localPosition = Vector3.zero;
        child.transform.localScale = Vector3.one;
        animationUI = child.AddComponent<AnimationUI>();
    }
    
    [UnityEditor.MenuItem("GameObject/UI (Canvas)/Create ButtonAnimationUI")]
    static void CreateButtonAnimationUI(UnityEditor.MenuCommand menuCommand) {
        GameObject selected = UnityEditor.Selection.activeGameObject;

        GameObject go = new GameObject("[ButtonAUI]");
        GameObject imgChild = new GameObject("[Image] Skin");
        GameObject txtChild = new GameObject("[Text] Text");
        GameObject auiGroup = new GameObject("[AnimationUIs]");
        imgChild.transform.SetParent(go.transform);
        txtChild.transform.SetParent(imgChild.transform);
        auiGroup.transform.SetParent(go.transform);
        
        var img = imgChild.AddComponent<Image>();
        img.color = new Color32(0x30,0x30,0x30,0xff);
     
        var txt = txtChild.AddComponent<TextMeshProUGUI>();
        txt.text = "Button";
        txt.alignment = TextAlignmentOptions.Center;
        txt.fontSize = 45;
        txt.fontStyle = FontStyles.Bold;
        txt.verticalAlignment = VerticalAlignmentOptions.Middle;



        ButtonAnimationUI button = go.AddComponent<ButtonAnimationUI>();

        button.transition = Transition.None;
        go.transform.SetParent(selected.transform);


        var rt = go.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(400, 80);
        rt.anchoredPosition = Vector2.zero;

        var imgRt = imgChild.GetComponent<RectTransform>();
        imgRt.sizeDelta = new Vector2(400, 80);
        imgRt.anchoredPosition = Vector2.zero;

        var txtRt = txt.GetComponent<RectTransform>();
        txtRt.anchoredPosition = Vector2.zero;
        txtRt.sizeDelta = new Vector2(400, 80);
        txtRt.anchorMin = Vector2.zero;
        txtRt.anchorMax = Vector2.one;
        txtRt.offsetMin = Vector2.zero;
        txtRt.offsetMax = Vector2.zero;
        

    }
#endif

}
