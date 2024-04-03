using System;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField] private InputManager input;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private FirstPersonController firstPersonController;
    private bool _pauseState { get; set; } = false;

    private void Awake()
    {
        pausePanel.SetActive(false);
    }

    private void Update()
    {
        if (input.GetEscapeButtonState())
        {
            if (_pauseState)
            {
                PauseExit();
            }
            if (!_pauseState)
            {
                PauseEnter();
            }
        }
    }

    private void PauseEnter()
    {
        _pauseState = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        firstPersonController.SetEnableMovement(false);
        firstPersonController.SetCameraLook(false);
    }
    public void PauseExit()
    {
        _pauseState = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        firstPersonController.SetEnableMovement(true);
        firstPersonController.SetCameraLook(true);
    }
}
