using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    public Transform borderCheckTransform;
    public Inventory inventory;

    private PlayerMovement playerMovement;
    private InputManager inputManager;
    private Vector3 hookShotPosition;
    private Vector3 hookShotDirection;
    private Vector3 hitPosition;
    private float hookShotSpeed = 10f;
    private float hookShotOffset = 1f;
    private float distanceToHitPoint;
    private float hookShotMinSpeed = 20f;
    private float hookShotMaxSpeed = 60f;
    private bool isOnBorder;
    private float maxDistanceShoot = 50f;

    private void Start()
    {
        InitializeInputs();
        InitializeComponents();
    }

    private void InitializeInputs()
    {
        inputManager = InputManager.Instance;
        inputManager.fireLeftAction.performed += ctx => HandleFireLeft();
        inputManager.fireRightAction.performed += ctx => HandleFireRight();
        inputManager.fireRightAction.canceled += ctx => CancelFireRight();
    }

    private void InitializeComponents()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (inputManager.fireRightAction.IsInProgress())
        {
            HandleHookShotInProgress();
        }
    }

    private void HandleFireLeft()
    {
        RaycastHit raycastHit;
        float currentWeaponDamage = inventory.currentWeaponObject.GetComponent<Weapon>().weaponData.weaponDamage;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, maxDistanceShoot))
        {
            if (raycastHit.collider.CompareTag("Enemy"))
            {
                Enemy target = raycastHit.collider.GetComponent<Enemy>();
                target.TakeDamage(currentWeaponDamage);
            }
        }
    }

    private void HandleFireRight()
    {
        HandleHookShotStart();
    }

    private void HandleHookShotStart()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit raycastHit))
        {
            hookShotPosition = raycastHit.point;
            Debug.Log("Hook Shot Position:" + hookShotPosition);
            hookShotDirection = (hookShotPosition - transform.position).normalized;
            playerMovement.state = PlayerMovement.PlayerState.HookShotFlying;
        }
    }

    private void HandleHookShotInProgress()
    {
        distanceToHitPoint = Vector3.Distance(transform.position, hookShotPosition);

        if (distanceToHitPoint  < hookShotOffset)
        {
            isOnBorder = CheckWallBorder();
            Debug.Log("isOnBorder:" + isOnBorder);
            if (isOnBorder)
            {
                ImpulseUp();
                inputManager.fireRightAction.Reset();
            }

            CancelFireRight();
        }

        hookShotSpeed = Mathf.Clamp(distanceToHitPoint, hookShotMinSpeed, hookShotMaxSpeed);
        playerMovement.CharacterController.Move(hookShotDirection * hookShotSpeed * Time.deltaTime);

    }

    private void CancelFireRight()
    {
        hookShotDirection = Vector3.zero;
        playerMovement.state = PlayerMovement.PlayerState.Normal;
        playerMovement.ResetGravity();
    }

    private bool CheckWallBorder()
    {
        return !Physics.Raycast(borderCheckTransform.position, borderCheckTransform.forward, 1f);
    }

    private void ImpulseUp()
    {
        playerMovement.CharacterController.Move(Vector3.up * 100 * Time.deltaTime);
        playerMovement.CharacterController.Move(borderCheckTransform.forward * 60 * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(borderCheckTransform.position, borderCheckTransform.forward.normalized * 1f, Color.green);
    }
}
