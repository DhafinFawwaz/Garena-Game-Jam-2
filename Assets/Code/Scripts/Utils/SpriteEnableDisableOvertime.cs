using System.Collections;
using UnityEngine;

public class SpriteEnableDisableOvertime : MonoBehaviour
{
    [SerializeField] float _period = 1f;
    [SerializeField] SpriteRenderer _sprite;
    [SerializeField] float _startDelay = 0f;
    void Reset() {
        _sprite = GetComponent<SpriteRenderer>();
    }

    void Awake() {
        StartCoroutine(Loop());
    }
    IEnumerator Loop() {
        yield return new WaitForSeconds(_startDelay);
        while (true) {
            yield return new WaitForSeconds(_period);
            _sprite.enabled = !_sprite.enabled;
        }
    }
}
