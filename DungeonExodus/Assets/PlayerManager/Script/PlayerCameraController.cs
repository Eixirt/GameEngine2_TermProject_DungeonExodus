using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraController : MonoBehaviour, PlayerInput.IPlayerCameraActions
{
    private PlayerInput _inputAction;
    private Vector2 _mouseInputVector;
    private float _cameraAngle;
    [SerializeField] private float Sensitive = 10f;
    [SerializeField] private Transform cameraTransform;
    
    
    private void OnEnable()
    {
        if(_inputAction == null)
            _inputAction = new PlayerInput();
        
        _inputAction.PlayerCamera.SetCallbacks(this);
        _inputAction.PlayerCamera.Enable();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        transform.Rotate(0f,_mouseInputVector.x * Time.deltaTime*Sensitive,0f);
        _cameraAngle = Mathf.Clamp( _cameraAngle - _mouseInputVector.y , -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(_cameraAngle, 0, 0);
    }
    private void OnDisable()
    {
        _inputAction.PlayerCamera.Disable();
    }
    public void OnMouse(InputAction.CallbackContext context)
    {
        _mouseInputVector = context.ReadValue<Vector2>();
    }
}
