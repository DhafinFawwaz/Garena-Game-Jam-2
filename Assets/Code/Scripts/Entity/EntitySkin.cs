using UnityEngine;

public class EntitySkin : MonoBehaviour
{
    [SerializeField] Sprite _rightDownSR;
    [SerializeField] Sprite _rightUpSR;
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] Transform _skin;
    [SerializeField] Transform _skinAlertAnchor;

    public void LookDirection(Vector2 dir) {
        if(dir.y < 0) {
            _renderer.sprite = _rightDownSR;
            _skin.localScale = new Vector3(dir.x < 0 ? -1 : 1, 1, 1);
        }
        else {
            _renderer.sprite = _rightUpSR;
            _skin.localScale = new Vector3(dir.x < 0 ? -1 : 1, 1, 1);
        }
    }

    public bool IsMoving { get; set; } = false;

    float _timer = 0;
    [SerializeField] float _jumpHeight = 0.1f;
    [SerializeField] float _jumpSpeed = 2f;
    [SerializeField] float _jumpTreshold = 0.1f; 


    float _startTimeOffset = 0f;
    void Start() {
        _startTimeOffset = Random.Range(0f, 1f);
        _timer = _startTimeOffset;
    }
    void Update() {
        if(!IsMoving && _skin.localPosition.y < _jumpTreshold) return;

        _timer += Time.deltaTime;
        float yOffset = Parabole((_timer * _jumpSpeed) % 1f) * _jumpHeight;
        Vector3 localPos = _skin.localPosition;
        localPos.y = yOffset;
        _skin.localPosition = localPos;
        if(_timer >= 1) _timer = 0f;
    }


    [SerializeField] float _alertJump = 0.5f;
    public void AlertJump(float x) {
        Vector3 localPos = _skinAlertAnchor.localPosition;
        localPos.y = Parabole(x) * _alertJump;
        _skinAlertAnchor.localPosition = localPos;
    }

    float Parabole(float x) {
        return -4f * (x - 0.5f) * (x - 0.5f) + 1f;
    }
    
}
