using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public RectTransform inventoryPanel;
    public int prevItemSelectedIndex;
    public int itemSelectedIndex;
    public Image[] weaponItems;
    public GameObject[] weaponObjects;
    public GameObject currentWeaponObject;

    private InputManager inputManager;
    private bool isActive = false;
    private Vector2 mousePosition;
    private float currentAngle;
    private Image prevWeaponItemSelected;
    private Image weaponItemSelected;

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
    }

    private void Start()
    {
        InitializeComponents();
        InitializeInputs();
        currentWeaponObject = weaponObjects[0];
        prevWeaponItemSelected = weaponItems[0];
        
    }

    private void InitializeComponents()
    {
        isActive = false;
        inventoryPanel.gameObject.SetActive(isActive);
    }
    private void InitializeInputs()
    {
        inputManager = InputManager.Instance;
        inputManager.inventoryAction.performed += ctx => ActiveInventory();
        inputManager.inventoryAction.canceled += ctx => DeactivateInventory();
    }

    private void Update()
    {
        if (!isActive) return;
        HandleInventory();

    }

    private void HandleInventory()
    {
        mousePosition = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2);
        currentAngle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
        currentAngle = (currentAngle + 360) % 360;
        itemSelectedIndex = (int)currentAngle / 90;
        if (itemSelectedIndex != prevItemSelectedIndex)
        {
            weaponItemSelected = weaponItems[itemSelectedIndex];
            prevWeaponItemSelected.color = Color.white;
            weaponItemSelected.color = Color.yellow;

            weaponObjects[prevItemSelectedIndex].SetActive(false);
            weaponObjects[itemSelectedIndex].SetActive(true);
            currentWeaponObject = weaponObjects[itemSelectedIndex];

            prevItemSelectedIndex = itemSelectedIndex;
            prevWeaponItemSelected = weaponItemSelected;
        }
    }

    private void ActiveInventory()
    {
        isActive = true;
        inventoryPanel.gameObject.SetActive(isActive);
        Cursor.lockState = CursorLockMode.None;
    }
    
    private void DeactivateInventory()
    {
        isActive = false;
        inventoryPanel.gameObject.SetActive(isActive);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
