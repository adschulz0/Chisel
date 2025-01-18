using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TestCOPY : MonoBehaviour
{
    private MeshFilter meshFilter;
    private Mesh mesh;

    private List<Vector3> meshVertices;
    private List<int> meshTriangles;

    private List<int> verticesWithinBounds;

    //NEW - PUSH VERTEX IN
    private List<Vector3> meshNormals;
    private float triangleLength;

    public float width;


    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;

        meshVertices = mesh.vertices.ToList();
        meshTriangles = mesh.triangles.ToList();

        //NEW - PUSH VERTEX IN
        meshNormals = mesh.normals.ToList();
        triangleLength = Vector3.Distance(meshVertices[0], meshVertices[1]);
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
            Vector3 boundSize = new Vector3(toolSize.x, toolSize.y, width); //0.05 width of the box just for third dimensionality, can change

            Bounds bounds = new Bounds(center.position, boundSize);

            verticesWithinBounds = new List<int>();

            for (int i = 0; i < meshVertices.Count; i++)
            {
                Vector3 vertex = meshVertices[i];

                Vector3 worldVertex = transform.TransformPoint(vertex);

                if (bounds.Contains(worldVertex))
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
                
                //NEW - PUSH VERTEX IN
                if (verticesWithinBounds.Contains(firstTri))
                {
                    Vector3 vertex = meshVertices[firstTri];
                    //Vector3 normal = meshNormals[firstTri];
                    Vector3 normal = other.transform.forward;

                    Vector3 backwardPosition = vertex + normal * triangleLength;

                    meshVertices[firstTri] = backwardPosition;
                }
                if (verticesWithinBounds.Contains(secondTri))
                {
                    Vector3 vertex = meshVertices[secondTri];
                    //Vector3 normal = meshNormals[secondTri];
                    Vector3 normal = other.transform.forward;

                    Vector3 backwardPosition = vertex + normal * triangleLength;

                    meshVertices[secondTri] = backwardPosition;
                }
                if (verticesWithinBounds.Contains(thirdTri))
                {
                    Vector3 vertex = meshVertices[thirdTri];
                    //Vector3 normal = meshNormals[thirdTri];
                    Vector3 normal = other.transform.forward;

                    Vector3 backwardPosition = vertex + normal * triangleLength;

                    meshVertices[thirdTri] = backwardPosition;
                }

                    //continue;

                newTriangles.Add(firstTri);
                newTriangles.Add(secondTri);
                newTriangles.Add(thirdTri);
            }

            meshTriangles = newTriangles;

            mesh.vertices = meshVertices.ToArray();
            mesh.triangles = meshTriangles.ToArray();

            GetComponent<MeshFilter>().mesh = mesh;

            GetComponent<MeshFilter>().mesh.RecalculateNormals();
        }
    }
}
