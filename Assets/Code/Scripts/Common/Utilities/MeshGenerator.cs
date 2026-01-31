using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator {

    /// <summary>
    /// Generate a new mesh with the given vertices. result is a 2D mesh. All z needs to be 0;
    /// </summary>
    /// <param name="vertices"></param>
    /// <returns></returns>
    public static Mesh Generate2DMesh(Vector3[] vertices) {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        int[] triangles = new int[(vertices.Length - 2) * 3];
        for (int i = 0; i < vertices.Length - 2; i++) {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        return mesh;

    }
}
