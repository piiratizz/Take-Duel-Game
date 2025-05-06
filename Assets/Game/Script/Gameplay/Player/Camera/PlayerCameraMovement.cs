using System;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class PlayerCameraMovement : NetworkBehaviour
{
    [SerializeField] private float _sensitivity = 5f;
    [SerializeField] private Transform _cameraPosition;
    [SerializeField] private Transform _forceLookTarget;
    
    [Inject] private PlayerCameraRoot _playerCamera;
    
    private bool _initialized;
    private bool _blocked;
    private bool _forcedLook;
    
    public void Initialize()
    {
        if (!isLocalPlayer) return;
        Cursor.lockState = CursorLockMode.Locked;
        
        _blocked = true;
        _initialized = true;
    }
    
    private void Update()
    {
        if (!isLocalPlayer || !_initialized) return;
        
        UpdateCameraPosition();

        if (_forcedLook)
        {
            UpdateCameraForceLooking();
        }
        
        if(_blocked) return;
        
        UpdateCameraRotation();
    }
    
    private void UpdateCameraPosition()
    {
        _playerCamera.transform.position = _cameraPosition.position;
    }
    
    private void UpdateCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * _sensitivity * Time.deltaTime; 
        float mouseY = Input.GetAxis("Mouse Y") * _sensitivity * Time.deltaTime;
        
        _playerCamera.transform.localRotation *= Quaternion.Euler(-mouseY, 0, 0);
        transform.Rotate(0,mouseX,0);
    }

    private void UpdateCameraForceLooking()
    {
        _playerCamera.transform.localRotation = Quaternion.LookRotation(_forceLookTarget.position);
    }

    public void SetCameraMovementTarget(Transform target)
    {
        _cameraPosition = target;
    }

    public void EnableForceLookingOnTarget(Transform target)
    {
        _forcedLook = true;
        _forceLookTarget = target;
    }
    
    public void DisableForceLookingOnTarget()
    {
        _forcedLook = false;
    }
    
    public void BlockMovement()
    {
        _blocked = true;
    }
    
    public void UnBlockMovement()
    {
        _blocked = false;
    }
}