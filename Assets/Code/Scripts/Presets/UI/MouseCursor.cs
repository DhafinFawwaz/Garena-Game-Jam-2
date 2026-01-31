using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MouseCursor : MonoBehaviour
{
    public static MouseCursor Main { get; private set; }
    void Awake()
    {
        Cursor.visible = false;
        Main = this;
        _cursorImg.sprite = _unclickedCursor;
    }
    
    [SerializeField] float normalScale = 0.3f;
    [SerializeField] float downScale = 0.1f;
    [SerializeField] Image _cursorImg;
    [SerializeField] Sprite _unclickedCursor;
    [SerializeField] Sprite _clickedCursor;
    [SerializeField] GameObject _targetObj;
    public Sprite ClickedCursor { get => _clickedCursor; set {
        _clickedCursor = value;
        if(_cursorImg.sprite == _clickedCursor)
            _cursorImg.sprite = value;
    } }
    public Sprite UnclickedCursor { get => _unclickedCursor; set {
        _unclickedCursor = value;
        if(_cursorImg.sprite == _unclickedCursor)
            _cursorImg.sprite = value;
    } }
    public Vector2 MousePosition { 
        get { 
            return ScreenToRectPos(_rt.parent as RectTransform, Mouse.current.position.ReadValue());
        }
    }

    public Vector2 ScreenToRectPos(RectTransform rectTransform, Vector2 screen_pos)
    { 
        Vector2 anchorPos = screen_pos - new Vector2(rectTransform.position.x, rectTransform.position.y);
        anchorPos= new Vector2(anchorPos.x / rectTransform.lossyScale.x, anchorPos.y / rectTransform.lossyScale.y);
        return anchorPos;
    }

    [SerializeField] RectTransform _rt;

    void Update()
    {
        _rt.anchoredPosition = MousePosition;
        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartCoroutine(LocalScaleAnimation(transform, transform.localScale, Vector3.one * downScale, 0.2f));
            _cursorImg.sprite = _clickedCursor;
        }
        else if(Mouse.current.leftButton.wasReleasedThisFrame)
        {
            StartCoroutine(LocalScaleAnimation(transform, transform.localScale, Vector3.one * normalScale, 0.2f));
            _cursorImg.sprite = _unclickedCursor;
        }
    }

    byte _key;
    IEnumerator LocalScaleAnimation(Transform trans, Vector3 start, Vector3 end, float duration)
    {
        byte requirement = ++_key;
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1 && requirement == _key)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            trans.localScale = Vector3.LerpUnclamped(start, end, Ease.OutQuart(t));
            yield return null;
        }
        if(requirement == _key)
            trans.localScale = end;
    }

    public void SetToCursor()
    {
        // if(_cursorImg.sprite == _unclickedCursor || _cursorImg.sprite == _clickedCursor) return;
        _cursorImg.sprite = _unclickedCursor;
        _cursorImg.enabled = true;
        _targetObj.SetActive(false);
    }

    public void SetToTarget()
    {
        _cursorImg.enabled = false;
        _targetObj.SetActive(true);
    }
}