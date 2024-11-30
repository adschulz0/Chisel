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

    public GameObject knife;

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;

        meshVertices = mesh.vertices.ToList();
        meshTriangles = mesh.triangles.ToList();


    }

    private void Update()
    {
        Transform knifeTransform = knife.transform;

        // Access the corner points
        Transform topLeft = knifeTransform.Find("Top Left");
        Transform topRight = knifeTransform.Find("Top Right");
        Transform bottomLeft = knifeTransform.Find("Bottom Left");
        Transform bottomRight = knifeTransform.Find("Bottom Right");

        Debug.DrawRay(topLeft.position, knifeTransform.forward, Color.green);
        Debug.DrawRay(topRight.position, knifeTransform.forward, Color.green);
        Debug.DrawRay(bottomLeft.position, knifeTransform.forward, Color.green);
        Debug.DrawRay(bottomRight.position, knifeTransform.forward, Color.green);

    }


    private void OnTriggerStay(Collider other)
    {
        if (other.name == "knife")
        {
            Transform knifeTransform = other.transform;

            // Access the corner points
            Transform topLeft = knifeTransform.Find("Top Left");
            Transform topRight = knifeTransform.Find("Top Right");
            Transform bottomLeft = knifeTransform.Find("Bottom Left");
            Transform bottomRight = knifeTransform.Find("Bottom Right");

            // Convert their world positions to the cube's local space
            Vector3 topLeftLocal = transform.InverseTransformPoint(topLeft.position);
            Vector3 topRightLocal = transform.InverseTransformPoint(topRight.position);
            Vector3 bottomLeftLocal = transform.InverseTransformPoint(bottomLeft.position);
            Vector3 bottomRightLocal = transform.InverseTransformPoint(bottomRight.position);


            // Find min and max bounds in local space
            float minX = Mathf.Min(topLeftLocal.x, bottomLeftLocal.x);
            float maxX = Mathf.Max(topRightLocal.x, bottomRightLocal.x);
            float minY = Mathf.Min(bottomLeftLocal.y, bottomRightLocal.y);
            float maxY = Mathf.Max(topLeftLocal.y, topRightLocal.y);

            List<int> verticesWithinBounds = new List<int>();

            for (int i = 0; i < meshVertices.Count; i++)
            {
                Vector3 vertex = meshVertices[i];

                if ((vertex.x >= minX && vertex.x <= maxX) && (vertex.y >= minY && vertex.y <= maxY))
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
