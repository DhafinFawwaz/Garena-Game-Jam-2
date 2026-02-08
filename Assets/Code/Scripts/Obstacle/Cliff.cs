using UnityEngine;

public class Cliff : MonoBehaviour
{
    [SerializeField] Transform[] _pointFallInto;
    
    public Transform GetPointFallInto(Transform requester) {
        Transform nearestPoint = null;
        float nearestDistSqr = float.MaxValue;
        foreach(var point in _pointFallInto) {
            float distSqr = (point.position - requester.position).sqrMagnitude;
            if(distSqr < nearestDistSqr) {
                nearestDistSqr = distSqr;
                nearestPoint = point;
            }
        }
        return nearestPoint;
    }
}