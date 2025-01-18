using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomInputAction : MonoBehaviour
{

    //public InputActionReference customButton;

    public InputActionReference customThumbstick;


    // Start is called before the first frame update
    void Start()
    {
        //customButton.action.started += ButtonWasPressed;
        //customButton.action.canceled += ButtonWasReleased;
        customThumbstick.action.performed += ThumbstickMoved;
    }
    /*
    void ButtonWasPressed(InputAction.CallbackContext context)
    {

    }
    
    void ButtonWasReleased(InputAction.CallbackContext context)
    {

    }
    */
    void ThumbstickMoved(InputAction.CallbackContext context)
    {
        Vector2 extractedVector = context.ReadValue<Vector2>();
        Debug.Log(extractedVector);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
