using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    private Mesh mesh;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;

        Debug.Log(mesh.triangles.Length);
        Debug.Log(mesh.triangles.Length / 3);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "knife")
        {
            mesh = GetComponent<MeshFilter>().mesh;

            Vector3[] originalVertices = mesh.vertices;
            int[] originalTriangles = mesh.triangles;

            List<Vector3> newVertices = new List<Vector3>();
            List<int> newTriangles = new List<int>();
            Dictionary<int, int> vertexMap = new Dictionary<int, int>();

            // Find the closest vertex to the knife
            Vector3 contactPoint = other.ClosestPoint(transform.position);
            float closestDistance = float.MaxValue;
            int closestVertexIndex = -1;

            for (int i = 0; i < originalVertices.Length; i++)
            {
                float distance = Vector3.Distance(transform.TransformPoint(originalVertices[i]), contactPoint);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestVertexIndex = i;
                }
            }

            // Rebuild vertices, excluding the closest one
            for (int i = 0; i < originalVertices.Length; i++)
            {
                if (i != closestVertexIndex)
                {
                    vertexMap[i] = newVertices.Count;
                    newVertices.Add(originalVertices[i]);
                }
            }

            // Rebuild triangles, remapping indices
            for (int i = 0; i < originalTriangles.Length; i += 3)
            {
                int v1 = originalTriangles[i];
                int v2 = originalTriangles[i + 1];
                int v3 = originalTriangles[i + 2];

                // Skip triangles referencing the removed vertex
                if (v1 == closestVertexIndex || v2 == closestVertexIndex || v3 == closestVertexIndex)
                    continue;

                newTriangles.Add(vertexMap[v1]);
                newTriangles.Add(vertexMap[v2]);
                newTriangles.Add(vertexMap[v3]);
            }

            // Assign the new mesh
            Mesh newMesh = new Mesh();
            newMesh.vertices = newVertices.ToArray();
            newMesh.triangles = newTriangles.ToArray();
            newMesh.RecalculateNormals();

            GetComponent<MeshFilter>().mesh = newMesh;
        }
    }
}
