using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Test : MonoBehaviour
{
    private MeshFilter meshFilter;
    private Mesh mesh;

    private List<Vector3> meshVertices;
    private List<int> meshTriangles;

    private List<int> verticesWithinBounds;

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;

        meshVertices = mesh.vertices.ToList();
        meshTriangles = mesh.triangles.ToList();
    }

    private void Update()
    {

    }


    private void OnTriggerStay(Collider other)
    {
        if (other.name == "tool")
        {
            Transform toolTransform = other.transform;
            Transform center = toolTransform.Find("Center");

            Vector3 toolSize = toolTransform.transform.localScale;
            Vector3 boundSize = new Vector3(toolSize.x, toolSize.y, 0.05f); //0.05 width of the box just for third dimensionality, can change

            Bounds bounds = new Bounds(center.position, boundSize);

            verticesWithinBounds = new List<int>();

            for (int i = 0; i < meshVertices.Count; i++)
            {
                Vector3 vertex = meshVertices[i];

                if (bounds.Contains(vertex))
                {
                    verticesWithinBounds.Add(i);
                }
            }

            List<int> newTriangles = new List<int>();

            for (int i = 0; i < meshTriangles.Count; i += 3)
            {
                int firstTri = meshTriangles[i];
                int secondTri = meshTriangles[i + 1];
                int thirdTri = meshTriangles[i + 2];

                if (verticesWithinBounds.Contains(firstTri) || verticesWithinBounds.Contains(secondTri) || verticesWithinBounds.Contains(thirdTri))
                {
                    continue;
                }

                newTriangles.Add(firstTri);
                newTriangles.Add(secondTri);
                newTriangles.Add(thirdTri);
            }

            meshTriangles = newTriangles;

            //mesh.vertices = meshVertices.ToArray();
            mesh.triangles = meshTriangles.ToArray();

            GetComponent<MeshFilter>().mesh = mesh;

            GetComponent<MeshFilter>().mesh.RecalculateNormals();
        }
    }
}
