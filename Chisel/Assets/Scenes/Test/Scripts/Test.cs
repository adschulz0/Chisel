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
        /*
        Vector3 boxOffset = knife.transform.forward / knife.transform.localScale.z / 2f * 0.9f;
        float castLength = knife.transform.localScale.z * 0.1f / 2f;

        Debug.DrawRay(knife.transform.localPosition + boxOffset, knife.transform.forward * castLength * 2, Color.red);

        if (Physics.Raycast(knife.transform.localPosition + boxOffset, knife.transform.forward, out RaycastHit hitInfo, castLength * 2))
        {
            Debug.Log(hitInfo.triangleIndex);
        }*/

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "knife")
        {
            Debug.Log("collided with knife");

            Vector3 boxOffset = other.transform.forward / other.transform.localScale.z / 2f * 0.9f;
            float castLength = other.transform.localScale.z * 0.1f / 2f;

            Vector3 boxHalfExtents = other.transform.localScale / 2f;
            boxHalfExtents.z = castLength / 2f;
            if (Physics.Raycast(other.transform.localPosition + boxOffset, other.transform.forward, out RaycastHit hitInfo, castLength))
            {
                Debug.Log("Knife ray hit box");

                Vector3 localHitPosition = meshFilter.transform.InverseTransformPoint(hitInfo.point);

                Debug.Log("local hit position: " + localHitPosition);

                
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = localHitPosition;
                cube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);  // Set the size of the cube

                float closestDistance = Mathf.Infinity;
                Vector3 closestVertex = Vector3.zero;
                int hitTriangle = 0;

                for (int i = 0; i < meshVertices.Count; i++)
                {
                    float distance = Vector3.Distance(meshVertices[i], localHitPosition);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestVertex = meshVertices[i];
                        hitTriangle = i;
                    }
                }

                Debug.Log(closestVertex);

                GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube2.transform.position = closestVertex;
                cube2.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);  // Set the size of the cube
                
                //NEW GOAL TO SEE IF VERTICES ARE WITHIN THE BOUNDS OF THE KNIFE'S BOX COLLIDER (OR SOME BOUNDING BOX)

                // Get the local position and size of the BoxCollider (this defines the "bounding box" around the hit point)
                Vector3 boxCenter = other.GetComponent<BoxCollider>().center;

                Debug.Log(boxCenter);

                Vector3 boxSize = other.GetComponent<BoxCollider>().size;

                Debug.Log(boxSize);


                List<int> newTriangles = new List<int>();

                for (int i = 0; i < meshTriangles.Count; i += 3)
                {
                    if (meshTriangles[i] == hitTriangle || meshTriangles[i + 1] == hitTriangle || meshTriangles[i + 2] == hitTriangle)
                    {
                        continue;
                    }

                    newTriangles.Add(meshTriangles[i]);
                    newTriangles.Add(meshTriangles[i + 1]);
                    newTriangles.Add(meshTriangles[i + 2]);
                }

                meshTriangles = newTriangles;
            }
            /*
            if (Physics.BoxCast(other.transform.localPosition + boxOffset, boxHalfExtents, other.transform.forward, out RaycastHit hitInfo, other.transform.localRotation, castLength))
            {
                Debug.Log("---");
                Debug.Log("triangle index: " + hitInfo.triangleIndex);

                int hitTriangle = meshTriangles[hitInfo.triangleIndex];

                Debug.Log("actual triangle: " + hitTriangle);

                Debug.Log("Triangle coordinates: " + meshVertices[hitTriangle]);

                List<int> newTriangles = new List<int>();

                for (int i = 0; i < meshTriangles.Count; i += 3)
                {
                    if (meshTriangles[i] == hitTriangle || meshTriangles[i + 1] == hitTriangle || meshTriangles[i + 2] == hitTriangle)
                    {
                        continue;
                    }

                    newTriangles.Add(meshTriangles[i]);
                    newTriangles.Add(meshTriangles[i + 1]);
                    newTriangles.Add(meshTriangles[i + 2]);
                }

                meshTriangles = newTriangles;

            }*/

            //mesh.vertices = meshVertices.ToArray();
            mesh.triangles = meshTriangles.ToArray();

            GetComponent<MeshFilter>().mesh = mesh;

            GetComponent<MeshFilter>().mesh.RecalculateNormals();
        }
    }
}
