using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DropSpawner : MonoBehaviour
{
    [SerializeField] Rigidbody2D[] _drops;
    [SerializeField] float _minForce = 2f;
    [SerializeField] float _maxForce = 5f;

    public void SpawnDrops(Vector2 position, int count = 10)
    {
        float angleStep = 360f / count;
        float startAngle = Random.Range(0f, 360f);

        for(int i = 0; i < count; i++) {
            Rigidbody2D drop = Instantiate(
                _drops[Random.Range(0, _drops.Length)],
                position,
                Quaternion.identity
            );

            float currentAngle = startAngle + i * angleStep;
            float variatedAngle = currentAngle + Random.Range(-angleStep * 0.3f, angleStep * 0.3f);
            Vector2 dir = new Vector2(Mathf.Cos(variatedAngle * Mathf.Deg2Rad), Mathf.Sin(variatedAngle * Mathf.Deg2Rad));

            float force = Random.Range(_minForce, _maxForce);
            drop.AddForce(dir * force, ForceMode2D.Impulse);
        }
    }

    public void SpawnDropsHere10() => SpawnDrops(transform.position, 10);
    
}

#if UNITY_EDITOR
[CustomEditor(typeof(DropSpawner))]
public class DropSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DropSpawner spawner = (DropSpawner)target;
        if (GUILayout.Button("Spawn 10 Drops Here"))
        {
            spawner.SpawnDropsHere10();
        }
    }
}

#endif