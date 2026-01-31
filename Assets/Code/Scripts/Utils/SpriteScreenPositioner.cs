using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteScreenPositioner : MonoBehaviour
{
    [SerializeField] Vector3 _viewportPosition;
    Camera _camera;
    void Awake()
    {
        _camera = Camera.main;
    }
    void Update()
    {
        Vector3 screenPosition = _camera.ViewportToWorldPoint(_viewportPosition);
        transform.position = screenPosition;
    }

    void OnDrawGizmos()
    {
        if(_camera == null) {
            _camera = Camera.main;
        }
        Vector3 screenPosition = _camera.ViewportToWorldPoint(_viewportPosition);
        transform.position = screenPosition;
    }
}