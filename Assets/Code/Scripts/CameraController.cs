using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Pan (WASD / Arrow Keys)")]
    [SerializeField] float _panSpeed = 0.5f;

    [Header("Zoom")]
    [SerializeField] [Range(0.01f, 0.5f)] float _zoomStep = 0.1f;
    [SerializeField] float _minZoom = 10f;
    [SerializeField] float _maxZoom = 500f;
    [SerializeField] float _zoomSmoothSpeed = 10f;

    [Header("References")]
    [SerializeField] CinemachineCamera _virtualCamera;

    Camera _cam;
    float _targetZoom;
    bool _isActive;

    void Awake()
    {
        _cam = Camera.main;
        if (_virtualCamera != null)
            _targetZoom = _virtualCamera.Lens.OrthographicSize;
        else
            _targetZoom = _cam.orthographicSize;
    }

    void OnEnable()
    {
        GameManager.S_OnGameStateChanged += HandleGameStateChanged;
    }

    void OnDisable()
    {
        GameManager.S_OnGameStateChanged -= HandleGameStateChanged;
    }

    void HandleGameStateChanged(GameState state)
    {
        _isActive = state == GameState.Playing;
    }

    void Update()
    {
        if (!_isActive) return;
        HandlePan();
        HandleZoom();
    }

    void HandlePan()
    {
        if (Keyboard.current == null) return;

        Vector3 moveDir = Vector3.zero;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            moveDir.y = 1f;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
            moveDir.y = -1f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            moveDir.x = -1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            moveDir.x = 1f;

        if (moveDir != Vector3.zero)
        {
            float currentSize = _virtualCamera != null ? _virtualCamera.Lens.OrthographicSize : _cam.orthographicSize;
            transform.position += moveDir.normalized * _panSpeed * currentSize * Time.deltaTime;
        }
    }

    void HandleZoom()
    {
        if (Mouse.current == null) return;

        float scroll = Mouse.current.scroll.ReadValue().y;
        if (Mathf.Abs(scroll) > 0.01f)
        {
            float factor = scroll > 0 ? (1f - _zoomStep) : (1f + _zoomStep);
            _targetZoom *= factor;
            _targetZoom = Mathf.Clamp(_targetZoom, _minZoom, _maxZoom);
        }

        if (_virtualCamera != null)
        {
            var lens = _virtualCamera.Lens;
            lens.OrthographicSize = Mathf.Lerp(lens.OrthographicSize, _targetZoom, Time.deltaTime * _zoomSmoothSpeed);
            _virtualCamera.Lens = lens;
        }
        else
        {
            _cam.orthographicSize = Mathf.Lerp(_cam.orthographicSize, _targetZoom, Time.deltaTime * _zoomSmoothSpeed);
        }
    }
}
