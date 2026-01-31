using UnityEngine;

public class Rotating : MonoBehaviour
{
    [SerializeField] float _period = 4;
    [SerializeField] Vector3 _axis = Vector3.up;

    void Update() {
        float angle = Time.time / _period * 360;
        transform.rotation = Quaternion.AngleAxis(angle, _axis);
    }
}
