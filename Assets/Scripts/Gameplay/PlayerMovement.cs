using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Transform playerBodyTransform;
    public CharacterController CharacterController { get; private set; }
    public SlideTrigger sliderTrigger;

    private InputManager inputManager;
    private Vector2 movementVector;
    private Vector3 movementDirection;
    private Vector3 playerVelocity;
    private float playerHeight = 2f;
    private float crouchHeight = 0.5f;
    private float gravity = -12f;
    public bool isGrounded;
    [SerializeField] private float movementSpeed = 8f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private LayerMask groundLayer;

    public PlayerState state;
    public enum PlayerState
    {
        Normal,
        HookShotFlying,
        WallSliding
    }

    private void Awake()
    {
        /*InputActionMap inputActionMap = InputActions.FindActionMap("Player Movement");
        moveAction = inputActionMap.FindAction("Move");
        jumpAction = inputActionMap.FindAction("Jump");
        sprintAction = inputActionMap.FindAction("Sprint");
        crouchAction = inputActionMap.FindAction("Crouch");*/
    }

    private void Start()
    {
        InitializeComponents();
        InitializeInputs();
    }

    private void InitializeComponents()
    {
        state = PlayerState.Normal;
        CharacterController = GetComponent<CharacterController>();
    }

    private void InitializeInputs()
    {
        inputManager = InputManager.Instance;

        inputManager.moveAction.performed += ctx => movementVector = ctx.ReadValue<Vector2>();
        inputManager.moveAction.canceled += ctx => CancelMove();

        inputManager.sprintAction.performed += ctx => HandleSprint();
        inputManager.sprintAction.canceled += ctx => CancelSprint();

        inputManager.crouchAction.performed += ctx => HandleCrouch();
        inputManager.crouchAction.canceled += ctx => CancelCrouch();

        inputManager.jumpAction.performed += ctx => HandleJump();
    }

    private void Update()
    {
        isGrounded = CheckGround();

        //CheckWallSliding();

        switch (state)
        {
            case PlayerState.Normal:
                HandleMovement();
                break;
            case PlayerState.HookShotFlying:
                break;
        }
    }

    private void HandleMovement()
    {
        movementDirection.x = movementVector.x;
        movementDirection.z = movementVector.y;

        CharacterController.Move(playerBodyTransform.TransformDirection(movementDirection) * movementSpeed * Time.deltaTime);

        playerVelocity.y += gravity * Time.deltaTime;

        if (isGrounded && playerVelocity.y < 0)
            ResetGravity();

        CharacterController.Move(playerVelocity * Time.deltaTime);
    }

    private void CancelMove()
    {
        movementVector = Vector2.zero;
    }

    private void HandleJump()
    {
        if (isGrounded)
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    private void HandleSprint()
    {
        movementSpeed *= 1.8f;
    }

    private void CancelSprint()
    {
        movementSpeed /= 1.8f;
    }

    private void HandleCrouch()
    {
        playerBodyTransform.localScale = new Vector3(playerBodyTransform.localScale.x, playerBodyTransform.localScale.y / 2, playerBodyTransform.localScale.z);
        CharacterController.height /= 2;
    }

    private void CancelCrouch()
    {
        playerBodyTransform.localScale = new Vector3(playerBodyTransform.localScale.x, playerBodyTransform.localScale.y * 2, playerBodyTransform.localScale.z);
        CharacterController.height *= 2;
    }

    public void ResetGravity()
    {
        playerVelocity.y = -2f;
    }

    private bool CheckGround()
    {
        return Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.4f, groundLayer);
    }

    private void CheckWallSliding()
    {
        if (sliderTrigger.isAbleToSlide)
        {

        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, Vector3.down.normalized * (playerHeight * 0.5f + 0.4f), Color.yellow);
    }
}
