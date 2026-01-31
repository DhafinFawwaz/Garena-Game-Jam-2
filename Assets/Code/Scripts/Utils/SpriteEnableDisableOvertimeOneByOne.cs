using System.Collections;
using UnityEngine;

public class SpriteEnableDisableOvertimeOneByOne : MonoBehaviour
{
    [SerializeField] float _period = 1f;
    [SerializeField] SpriteRenderer[] _sprite;
    [SerializeField] float _startDelay = 0f;

    void Awake() {
        StartCoroutine(Loop());
    }
    void DisableAll() {
        foreach (var sprite in _sprite) {
            sprite.enabled = false;
        }
    }
    IEnumerator Loop() {
        yield return new WaitForSeconds(_startDelay);

        while(true) {
            for(int i = 0; i < _sprite.Length; i++) {
                DisableAll();
                _sprite[i].enabled = true;
                yield return new WaitForSeconds(_period);
            }
        }
    }
}
