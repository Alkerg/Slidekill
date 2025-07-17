using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform playerBody;
    public Vector2 lookVector;

    private InputManager inputManager;
    private float mouseX;
    private float mouseY;
    private float verticalRotation;
    private float verticalClamp = 80f;
    private float sensitivity = 20f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        InitializeInputs();
    }

    private void InitializeInputs()
    { 
        inputManager = InputManager.Instance;

        inputManager.lookAction.performed += ctx => lookVector = ctx.ReadValue<Vector2>();
        inputManager.lookAction.canceled += ctx => lookVector = Vector2.zero;
    }

    private void Update()
    {
        RotateCamera();
    }

    private void RotateCamera()
    {
        mouseX = lookVector.x * sensitivity * Time.deltaTime;
        mouseY = lookVector.y * sensitivity * Time.deltaTime;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalClamp, verticalClamp);
        transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}
