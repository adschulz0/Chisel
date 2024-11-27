using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private Mesh mesh;

    private Vector3 knifeDimensions;
    private Vector3 boxCastDimensions;

    private RaycastHit hitInfo;

    private Vector3 boxHalfExtents;

    public GameObject knife;

    private Vector3 rayOffset;
    private float castLength;

    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;

        Vector3[] meshVertices = mesh.vertices;
        int[] meshTriangles = mesh.triangles;

        boxHalfExtents = knife.transform.localScale / 2f;
        boxHalfExtents.z = castLength;
    }

    private void Update()
    {
        //Vector3 lengthHalf = new Vector3(knife.transform.position.x, knife.transform.position.y, knife.transform.position.z * 2f * 0.9f);

        rayOffset = knife.transform.forward / knife.transform.localScale.z / 2f * 0.9f;

        Vector3 rayStart = knife.transform.localPosition + rayOffset;

        castLength = knife.transform.localScale.z * 0.1f / 2f;

        boxHalfExtents.z = castLength / 2f;

        Debug.DrawRay(rayStart, knife.transform.forward * castLength);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "knife")
        {
            Vector3 boxOffset = other.transform.forward / other.transform.localScale.z / 2f * 0.9f;
            float castLength = other.transform.localScale.z * 0.1f / 2f;


            if(Physics.BoxCast(other.transform.localPosition + boxOffset, boxHalfExtents, other.transform.forward, out hitInfo, other.transform.localRotation, castLength))
            {
                Debug.Log("Boxcast hit!");
            }
            if (Physics.Raycast(other.transform.localPosition + boxOffset, other.transform.forward, out hitInfo, castLength))
            {
                Debug.Log("Raycast hit!");
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Ensure that knifeTransform is valid before trying to draw
        if (knife.transform != null)
        {
            // Calculate the center of the boxcast
            Vector3 boxCenter = knife.transform.position + rayOffset;

            // Draw the wireframe box at the box's calculated position and scale
            Gizmos.color = Color.green;  // Set the color for the Gizmo
            Gizmos.DrawWireCube(boxCenter, boxHalfExtents * 2);  // Draw the wireframe cube (box)

            // Optionally, draw a line showing the direction of the box cast
            Gizmos.color = Color.red;  // Set color for the direction line
            Gizmos.DrawLine(boxCenter, boxCenter + knife.transform.forward * castLength);  // Line showing direction of the cast
        }
    }
}
