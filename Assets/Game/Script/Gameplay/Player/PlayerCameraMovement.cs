using System;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class PlayerCameraMovement : NetworkBehaviour
{
    [SerializeField] private float _sensitivity = 5f;
    [SerializeField] private Transform _cameraPosition;
    private PlayerCameraRoot _playerCamera;
    private bool _initialized;
    public void Initialize(PlayerCameraRoot playerCamera)
    {
        _playerCamera = playerCamera;
        
        if (!isLocalPlayer) return;
        Cursor.lockState = CursorLockMode.Locked;
        
        _initialized = true;
    }
    
    
    private void Update()
    {
        if (!isLocalPlayer || !_initialized) return;
        
        UpdateCameraPosition();
    }
    
    private void UpdateCameraPosition()
    {
        _playerCamera.transform.position = _cameraPosition.position;
        float mouseX = Input.GetAxis("Mouse X") * _sensitivity * Time.deltaTime; 
        float mouseY = Input.GetAxis("Mouse Y") * _sensitivity * Time.deltaTime;
        
        _playerCamera.transform.localRotation *= Quaternion.Euler(-mouseY, 0, 0);
        transform.Rotate(0,mouseX,0);
    }
}