using System;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class PlayerCameraMovement : NetworkBehaviour
{
    [SerializeField] private float _sensitivity = 5f;
    [SerializeField] private Transform _cameraPosition;
    [FormerlySerializedAs("_camera")] [SerializeField] private PlayerCameraRoot playerCamera;
    
    private void Start()
    {
        if (!isLocalPlayer) return;
        
        Cursor.lockState = CursorLockMode.Locked;
        playerCamera.transform.position = _cameraPosition.position;
    }

    
    private void Update()
    {
        UpdateCameraPosition();
    }
    
    private void UpdateCameraPosition()
    {
        if (!isLocalPlayer) return;
        
        playerCamera.transform.position = _cameraPosition.position;
        float mouseX = Input.GetAxis("Mouse X") * _sensitivity * Time.deltaTime; 
        float mouseY = Input.GetAxis("Mouse Y") * _sensitivity * Time.deltaTime;
        
        playerCamera.transform.localRotation *= Quaternion.Euler(-mouseY, 0, 0);
        transform.Rotate(0,mouseX,0);
    }
}