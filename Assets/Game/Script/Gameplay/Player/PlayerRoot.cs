using System;
using Mirror;
using UnityEngine;

public class PlayerRoot : NetworkBehaviour
{
    [SerializeField] private CharacterIKController _characterIKController;
    [SerializeField] private Camera _cameraObject;
    [SerializeField] private CameraRoot _cameraRoot;
    
    private void Start()
    {
        _cameraObject.gameObject.SetActive(false);
        _characterIKController.Initialize(_cameraRoot.BodyAimTarget);
        
        if (!isLocalPlayer) return;
        
        _cameraObject.gameObject.SetActive(true);
        
    }
}