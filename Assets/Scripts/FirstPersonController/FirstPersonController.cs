//Check: Artem is femboychik. Answer: Yes

using FMOD.Studio;
using UnityEngine;
using FMODUnity;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class FirstPersonController : MonoBehaviour
{
    //Get need components
    [SerializeField] private InputManager input;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private RectTransform staminaLine;
    //Settings
    [SerializeField] private bool enableMovement = true;
    [SerializeField] private bool enableCameraLook = true;
    [SerializeField] private float walkSpeed = 600f;
    [SerializeField] private float sprintSpeed = 800f;
    [SerializeField] private float crouchSpeed = 400f;
    [SerializeField] private float mouseSensitivity = 0.5f;
    [SerializeField] private bool enableXClamp = true;
    [SerializeField, Range(-360, 360)] private float maxCameraX = 60f;
    [SerializeField, Range(-360, 360)] private float minCameraX = -60f;
    [SerializeField] private float JumpForce = 1200f;
    //Mutable Variables
    private float currentSpeed { get; set; } = 600f;
    private Vector3 originalScale { get; set; }
    private Vector2 _playerMovementVector { get; set; }
    private Vector2 _cameraLookVector { get; set; }
    private float _cameraLookX { get; set; }
    private float _cameraLookY { get; set; }
    private bool _isCrouched { get; set; } = false;
    private bool _isSprinted { get; set; } = false;
    private float _stamina { get; set; } = 6f;
    private bool sprintAllowed { get; set; } = true;
    private int time { get; set;}
    private bool _isGrounded()
    {
        //Create a raycast
        RaycastHit hit;
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * 0.5f), transform.position.z);
        float distance = 0.75f;
        //Check a raycast
        if (Physics.Raycast(origin, transform.TransformDirection(Vector3.down), out hit, distance))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        currentSpeed = walkSpeed;
        originalScale = transform.localScale;
    }
    private void Update()
    {
        // Check: Movement is enable?
        if (enableMovement)
        {
            _playerMovementVector = input.GetMovementVector();
            PlayerMove(_playerMovementVector);
        }
        //Check: Movement is enable?
        if (enableCameraLook)
        {
            _cameraLookVector = input.GetCameraLookVector();
            CameraLook(_cameraLookVector);
        }
        if (input.GetJumpButtonState())
        {
            Jump(JumpForce);
        }
        Sprint(input.GetSprintButtonState(), sprintSpeed);
        Crouch(input.GetCrouchButtonState(), crouchSpeed);
        if (!_isSprinted && !_isCrouched)
        {
            currentSpeed = walkSpeed;
        }
        if (input.GetTemporaryDeviceState()) { TimeTravel(); }
    }
    private void PlayerMove(Vector2 playerMovementVector)
    {
        //Getting and adding movement values
        float moveX = playerMovementVector.x;
        float moveZ = playerMovementVector.y;
        Vector3 moveVector = moveX * transform.right + moveZ * transform.forward;
        rb.velocity = moveVector * currentSpeed * Time.deltaTime;
    }

    private void CameraLook(Vector2 cameraLookVector)
    {
        //Getting and adding camera look values
        _cameraLookX -= cameraLookVector.y * mouseSensitivity;
        _cameraLookY += cameraLookVector.x * mouseSensitivity;
        if (enableXClamp) {_cameraLookX = Mathf.Clamp(_cameraLookX, minCameraX,maxCameraX);}
        playerCamera.transform.localEulerAngles = new Vector3(_cameraLookX, 0, 0);
        transform.eulerAngles = new Vector3(0,_cameraLookY,0);
    }
    private void Jump(float force)
    {
        if (_isGrounded())
        {
            rb.AddForce(0,force,0);
        }
    }

    private void Sprint(bool enable, float speed)
    {
        if (enable && _isGrounded() && !_isCrouched && sprintAllowed)
        {
            _isSprinted = true;
            currentSpeed = speed;
            _stamina -= 1 * Time.deltaTime;
            float normalizedValue = Mathf.InverseLerp(0, 6, _stamina);
            float result = Mathf.Lerp(0, 1, normalizedValue);
            staminaLine.localScale = new Vector2(result, 1);
        }
        else
        {
            _isSprinted = false;
            if (_stamina < 6f)
            {
                _stamina += 1 * Time.deltaTime;
                float normalizedValue = Mathf.InverseLerp(0, 6, _stamina);
                float result = Mathf.Lerp(0, 1, normalizedValue);
                staminaLine.localScale = new Vector2(result, 1);
            }
        } 
        if (_stamina > 6f) { _stamina = 6f; }
        if (_stamina < 0f) { _stamina = 0f; }
        if (_stamina < 2f && !_isSprinted) { sprintAllowed = false; }
        if (_stamina >= 2f) { sprintAllowed = true; }
    }

    private void Crouch(bool enable, float speed)
    {
        if (enable && _isGrounded() && !_isSprinted)
        {
            transform.localScale = new Vector3(originalScale.x, originalScale.y / 2, originalScale.z);
            currentSpeed = speed;
            _isCrouched = true;
        }
        else
        {
            transform.localScale = originalScale;
            _isCrouched = false;
        }
    }

    private void TimeTravel()
    {
        switch (time)
        {
            case 0:
                transform.position = new Vector3(transform.position.x + 26, transform.position.y, transform.position.z);
                time = 1;
                break;
            case 1:
                transform.position = new Vector3(transform.position.x - 26, transform.position.y, transform.position.z);
                time = 0;
                break;
        }
    }
    public void SetEnableMovement(bool state)
    {
        enableMovement = state;
    }
    public void SetCameraLook(bool state)
    {
        enableCameraLook = state;
    }
}
