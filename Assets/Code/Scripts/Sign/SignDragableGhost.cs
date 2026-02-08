using UnityEngine;

public class SignDragableGhost : MonoBehaviour
{
    [SerializeField] Sprite _ghostSprite;
    public Sprite GhostSprite { get => _ghostSprite; set => _ghostSprite = value; }
    [SerializeField] float _ghostAlpha = 0.5f;
    [SerializeField] float _ghostScale = 1f;

    SpriteRenderer _spawnedGhostRenderer = null;
    public void SpawnGhost(Vector2 position) {
        if(_spawnedGhostRenderer == null) {
            GameObject ghostObj = new GameObject("SignGhost");
            ghostObj.transform.position = position;
            _spawnedGhostRenderer = ghostObj.AddComponent<SpriteRenderer>();
            _spawnedGhostRenderer.sprite = _ghostSprite;
            _spawnedGhostRenderer.transform.localScale = Vector3.one * _ghostScale;
            Color c = _spawnedGhostRenderer.color;
            c.a = _ghostAlpha;
            _spawnedGhostRenderer.color = c;
        }
        else {
            _spawnedGhostRenderer.transform.position = position;
        }
    }

    public void DestroyGhost() {
        if(_spawnedGhostRenderer == null) return;
        Destroy(_spawnedGhostRenderer.gameObject);
        _spawnedGhostRenderer = null;
    }

    public void MoveGhost(Vector2 position) {
        if(_spawnedGhostRenderer == null) return;
        _spawnedGhostRenderer.transform.position = position;
    }
}
