using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;
    public Canvas pauseMenu;

    private InputManager inputManager;
    private bool isPaused = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        InitializeInputs();
        InitializeComponents();
    }

    private void InitializeInputs()
    {
        inputManager = InputManager.Instance;
        inputManager.pauseAction.performed += ctx => HandlePause();
    }

    private void InitializeComponents()
    {
        pauseMenu.enabled = false;
    }

    void Update()
    {
        
    }

    private void HandlePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            pauseMenu.enabled = true;
        }
        else
        {
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            pauseMenu.enabled = false;
        }

        inputManager.SetActionMap(isPaused);
    }
}
