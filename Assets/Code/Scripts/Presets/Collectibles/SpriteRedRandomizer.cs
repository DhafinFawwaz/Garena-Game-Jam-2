using UnityEngine;

public class SpriteRedRandomizer : MonoBehaviour
{
    [SerializeField] SpriteRenderer _sprite;
    void Awake() {
        Color c = _sprite.color;
        c.r = Random.Range(0.5f, 1f);
        _sprite.color = c;
    }

    void Reset() {
        _sprite = GetComponent<SpriteRenderer>();
    }
}
