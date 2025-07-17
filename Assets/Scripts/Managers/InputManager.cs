using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public static InputManager Instance;

    public InputActionAsset InputActions;
    public InputAction moveAction { get; private set; }
    public InputAction jumpAction { get; private set; }
    public InputAction lookAction { get; private set; }
    public InputAction fireLeftAction { get; private set; }
    public InputAction fireRightAction { get; private set; }
    public InputAction sprintAction { get; private set; }
    public InputAction crouchAction { get; private set; }
    public InputAction pauseAction { get; private set; }
    public InputAction inventoryAction { get; private set; }

    private InputActionMap playerMovementActionMap;
    private InputActionMap extraKeysActionMap;

    private void Awake()
    {

        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        playerMovementActionMap = InputActions.FindActionMap("Player Movement");
        extraKeysActionMap = InputActions.FindActionMap("Extra Keys");

        moveAction = playerMovementActionMap.FindAction("Move");
        jumpAction = playerMovementActionMap.FindAction("Jump");
        sprintAction = playerMovementActionMap.FindAction("Sprint");
        crouchAction = playerMovementActionMap.FindAction("Crouch");
        lookAction = playerMovementActionMap.FindAction("Look");
        fireLeftAction = playerMovementActionMap.FindAction("FireLeft");
        fireRightAction = playerMovementActionMap.FindAction("FireRight");
        pauseAction = extraKeysActionMap.FindAction("Pause");
        inventoryAction = extraKeysActionMap.FindAction("Inventory");
    }
    private void Start()
    {
        
    }

    private void OnEnable()
    {
        playerMovementActionMap.Enable();
        extraKeysActionMap.Enable();
    }

    private void OnDisable()
    {
        playerMovementActionMap.Disable();
        extraKeysActionMap.Disable();
    }

    public void SetActionMap(bool isPaused)
    {
        if (isPaused)
            playerMovementActionMap.Disable();
        else
            playerMovementActionMap.Enable();
    }
}
