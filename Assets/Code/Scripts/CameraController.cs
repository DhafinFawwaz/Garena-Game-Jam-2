using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Pan (WASD / Arrow Keys)")]
    [SerializeField] float _panSpeed = 0.5f;

    [Header("Zoom")]
    [SerializeField][Range(0.01f, 0.5f)] float _zoomStep = 0.1f;
    [SerializeField] float _minZoom = 10f;
    [SerializeField] float _maxZoom = 500f;
    [SerializeField] float _zoomSmoothSpeed = 10f;

    [Header("Confiner")]
    [SerializeField] Collider2D _boundaryCollider;

    Camera _cam;
    float _targetZoom;
    bool _isActive;

    void Awake()
    {
        _cam = Camera.main;
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
            transform.position += moveDir.normalized * _panSpeed * _cam.orthographicSize * Time.deltaTime;
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

        _cam.orthographicSize = Mathf.Lerp(_cam.orthographicSize, _targetZoom, Time.deltaTime * _zoomSmoothSpeed);
    }

    void LateUpdate()
    {
        if (_boundaryCollider == null) return;
        ConfineCamera();
    }

    void ConfineCamera()
    {
        float camHeight = _cam.orthographicSize;
        float camWidth = camHeight * _cam.aspect;

        Bounds bounds = _boundaryCollider.bounds;

        float clampedX = Mathf.Clamp(transform.position.x, bounds.min.x + camWidth, bounds.max.x - camWidth);
        float clampedY = Mathf.Clamp(transform.position.y, bounds.min.y + camHeight, bounds.max.y - camHeight);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}
