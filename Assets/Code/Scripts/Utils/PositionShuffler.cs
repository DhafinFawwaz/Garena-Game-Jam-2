using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PositionShuffler : MonoBehaviour
{
    [SerializeField] Transform[] _objectsToShuffle;
    void Awake() {
        ShufflePositions();
    }

    void ShufflePositions() {
        Vector3[] originalPositions = _objectsToShuffle.Select(obj => obj.position).ToArray();
        Stack<Vector3> shuffledPositions = new Stack<Vector3>(originalPositions.OrderBy(_ => Random.value));
        for (int i = 0; i < _objectsToShuffle.Length; i++) {
            _objectsToShuffle[i].position = shuffledPositions.Pop();
        }
    }
}
