using System;
using Mirror;
using UnityEngine;
using Zenject;

public class CameraMovement : NetworkBehaviour
{
    [SerializeField] private float _sensitivity = 5f;
    [SerializeField] private Transform _cameraPosition;
    [SerializeField] private CameraRoot _camera;
    
    private void Start()
    {
        if (!isLocalPlayer) return;
        
        Cursor.lockState = CursorLockMode.Locked;
        _camera.transform.position = _cameraPosition.position;
    }

    
    private void Update()
    {
        UpdateCameraPosition();
    }
    
    private void UpdateCameraPosition()
    {
        if (!isLocalPlayer) return;
        
        _camera.transform.position = _cameraPosition.position;
        float mouseX = Input.GetAxis("Mouse X") * _sensitivity * Time.deltaTime; 
        float mouseY = Input.GetAxis("Mouse Y") * _sensitivity * Time.deltaTime;
        
        _camera.transform.localRotation *= Quaternion.Euler(-mouseY, 0, 0);
        transform.Rotate(0,mouseX,0);
    }
}