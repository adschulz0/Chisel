using UnityEngine;
using UnityEngine.XR;

public class XRCharacterController : MonoBehaviour
{
    public CharacterController characterController;
    public Transform xrCamera;

    void Start()
    {
        if (characterController == null)
            characterController = GetComponent<CharacterController>();

        if (xrCamera == null)
            xrCamera = Camera.main.transform;
    }

    void Update()
    {
        // Match Character Controller's center with the XR Camera's position
        Vector3 center = xrCamera.position - transform.position;
        center.y = characterController.height / 2;
        characterController.center = center;
    }
}
