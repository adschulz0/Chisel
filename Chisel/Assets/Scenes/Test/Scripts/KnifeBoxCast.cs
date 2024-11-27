using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeBoxCast : MonoBehaviour
{
    private Vector3 knifeDimensions;

    private void Update()
    {
        PerformBoxCast();
    }

    private void PerformBoxCast()
    {
        // Get the knife's position, dimensions, and orientation
        Transform knifeTransform = transform; // Assuming this script is on the knife GameObject
        knifeDimensions = knifeTransform.localScale; // Get the dimensions (local scale)

        // Define BoxCast parameters
        Vector3 boxCenter = knifeTransform.position; // Center of the box
        boxCenter.z -= boxCenter.z / 2;
        Vector3 boxHalfExtents = knifeDimensions / 2; // Half extents of the knife (for BoxCast)
        Vector3 boxDirection = knifeTransform.forward; // Direction of the BoxCast (local forward)
        Quaternion boxRotation = knifeTransform.rotation; // Orientation of the box

        // Perform the BoxCast
        if (Physics.BoxCast(
            boxCenter,
            boxHalfExtents,
            boxDirection,
            out RaycastHit hitInfo,
            boxRotation,
            knifeDimensions.z)) // Cast length is the knife's z-dimension
        {
            Debug.Log($"BoxCast hit: {hitInfo.collider.name} at position {hitInfo.point}");
        }
    }

    private void OnDrawGizmos()
    {
        // Draw the BoxCast visualization
        Transform knifeTransform = transform; // Assuming this script is on the knife GameObject

        // Set up box parameters for visualization
        Vector3 boxCenter = knifeTransform.position;
        Vector3 boxHalfExtents = knifeTransform.localScale / 2;
        Quaternion boxRotation = knifeTransform.rotation;

        // Draw the box using Gizmos
        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, boxRotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, knifeTransform.localScale);
    }
}
