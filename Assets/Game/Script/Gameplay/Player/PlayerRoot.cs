using System;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerRoot : NetworkBehaviour
{
    [SerializeField] private CharacterIKController _characterIKController;
    [SerializeField] private Camera _cameraObject;
    [SerializeField] private PlayerCameraRoot _playerCameraRoot;
    
    private void Start()
    {
        AttachCameraToLocalPlayer();
    }

    private void AttachCameraToLocalPlayer()
    {
        _cameraObject.gameObject.SetActive(false);
        _characterIKController.Initialize(_playerCameraRoot.BodyAimTarget);
        
        if (!isLocalPlayer) return;
        
        _cameraObject.gameObject.SetActive(true);
    }
}