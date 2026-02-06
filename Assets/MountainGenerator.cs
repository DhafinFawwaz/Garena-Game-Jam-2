using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MountainGenerator : MonoBehaviour
{
    public int resolution = 30;
    public float size = 60f;
    public float height = 40f;

    void Start()
    {
        GenerateMountain();
    }

    void GenerateMountain()
    {
        Mesh mesh = new Mesh();
        mesh.name = "MountainMesh";
        Vector3[] vertices = new Vector3[(resolution + 1) * (resolution + 1)];
        Vector2[] uvs = new Vector2[vertices.Length];
        int[] triangles = new int[resolution * resolution * 6];

        for (int i = 0, y = 0; y <= resolution; y++)
        {
            for (int x = 0; x <= resolution; x++, i++)
            {
                float xPerc = (float)x / resolution;
                float yPerc = (float)y / resolution;
                float dist = Vector2.Distance(new Vector2(xPerc, yPerc), new Vector2(0.5f, 0.5f));
                
                // Cone shape drop-off
                float h = Mathf.Clamp01(1f - (dist * 2f));
                h = Mathf.SmoothStep(0, 1, h) * height;
                
                // Layered Perlin Noise for "Craggy" look
                float noise = Mathf.PerlinNoise(xPerc * 4f, yPerc * 4f) * 0.5f;
                noise += Mathf.PerlinNoise(xPerc * 10f, yPerc * 10f) * 0.2f;
                h += noise * h;

                vertices[i] = new Vector3((xPerc - 0.5f) * size, h, (yPerc - 0.5f) * size);
                uvs[i] = new Vector2(xPerc, yPerc);
            }
        }

        int vert = 0;
        int tri = 0;
        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                triangles[tri + 0] = vert + 0;
                triangles[tri + 1] = vert + resolution + 1;
                triangles[tri + 2] = vert + 1;
                triangles[tri + 3] = vert + 1;
                triangles[tri + 4] = vert + resolution + 1;
                triangles[tri + 5] = vert + resolution + 2;

                vert++;
                tri += 6;
            }
            vert++;
        }

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        
        GetComponent<MeshFilter>().mesh = mesh;
        
        // Add a mesh collider
        if (gameObject.GetComponent<MeshCollider>() == null)
            gameObject.AddComponent<MeshCollider>().sharedMesh = mesh;
    }
}